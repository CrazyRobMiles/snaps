using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

using Windows.Foundation;
using System.Reflection;
using Windows.System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SnapsLibrary;

namespace XAMLSnaps
{
    public partial class SnapsManager
    {
        public static CoreDispatcher UICoreDispatcher = null;

        public Task InvokeOnUIThread(Action a)
        {
            return UICoreDispatcher.RunAsync(CoreDispatcherPriority.Normal, () => a()).AsTask();
        }

        // work in progres
        public Task InvokeOnUI(Action a)
        {
            if (UICoreDispatcher.HasThreadAccess)
                return ThreadPool.RunAsync(delegate { a(); }).AsTask();
            else
                return UICoreDispatcher.RunAsync(CoreDispatcherPriority.Normal, () => a()).AsTask();
        }

        public Grid DisplayGrid;

        private int debugOption;

        private static SnapsManager manager = null;

        public static SnapsManager ActiveSnapsManager
        {
            get
            {
                return manager;
            }
        }

        public static SnapsManager SetupManager(Grid displayGrid)
        {
            return SnapsManager.SetupManager(CoreWindow.GetForCurrentThread().Dispatcher, displayGrid);
        }

        public static SnapsManager SetupManager(CoreDispatcher dispatcher, Grid xamlPanel)
        {
            int debugOption = 0;
            manager = new SnapsManager(dispatcher, xamlPanel, debugOption);
            return manager;
        }

        public SnapsManager(CoreDispatcher dispatcher, Grid xamlPanel, int inDebugOption)
        {
            UICoreDispatcher = dispatcher;
            DisplayGrid = xamlPanel;

            buildDemoNameMappings();

            debugOption = inDebugOption;

            buildDisplay(xamlPanel, debugOption);

            selectDemo = getMethod(typeof(SnapsManager), "DemoSelection");
        }

        #region Demo Management

        Dictionary<string, Dictionary<string, MethodInfo>> nameMappings = new Dictionary<string, Dictionary<string, MethodInfo>>();

        public List<string> GetDemoFolders ()
        {
            List<string> result = new List<string>();
            foreach (var item in nameMappings.Keys)
            {
                result.Add(item.ToString());
            }
            result.Sort();

            // Move the item at the end of the list to the top
            int lastIndex = result.Count - 1;
            string lastName = result[lastIndex];
            result.RemoveAt(lastIndex);
            result.Insert(0, lastName);
            return result;
        }

        public List<string> GetDemosForGroup(string name)
        {
            List<string> result = new List<string>();
            if (!nameMappings.ContainsKey(name))
                return result;

            var groupList = nameMappings[name];

            foreach (var item in groupList.Keys)
            {
                result.Add(item.ToString());
            }
            result.Sort();
            return result;
        }

        public MethodInfo GetMethodInfoForApp(string group,string name)
        {
            if (!nameMappings.ContainsKey(group))
                return null;

            var groupMappings = nameMappings[group];

            if (!groupMappings.ContainsKey(name))
                return null;

            return groupMappings[name];
        }

        private void buildDemoNameMappings()
        {
            AssemblyName name = new AssemblyName("BeginToCodeWithCsharp");
            Assembly a = Assembly.Load(name);

            Regex chapter = new Regex("^Ch[0-9][0-9]_[0-9][0-9].*");
            Regex splitChapter = new Regex("^(?<chap>Ch[0-9][0-9])");
            Regex splitChapterNumber = new Regex("^Ch(?<no>[0-9][0-9])");

            foreach (Type t in a.GetTypes())
            {
                MethodInfo startProgramMethodInfo = getMethod(t, "StartProgram");

                if (startProgramMethodInfo == null)
                    continue;

                if (chapter.IsMatch(t.Name))
                {
                    Match m = splitChapter.Match(t.Name);
                    string chap = m.Result("${chap}");
                    Match p = splitChapterNumber.Match(t.Name);
                    string chapNo = p.Result("${no}");
                    string fullChapter = "Chapter " + chapNo;
                    if (!nameMappings.ContainsKey(fullChapter))
                    {
                        nameMappings.Add(fullChapter, new Dictionary<string, MethodInfo>());
                    }
                    var chapDictionary = nameMappings[fullChapter];
                    chapDictionary.Add(t.Name, startProgramMethodInfo);
                    continue;
                }

                string codeFolder = "My Snaps apps";
                if (!nameMappings.ContainsKey(codeFolder))
                {
                    nameMappings.Add(codeFolder, new Dictionary<string, MethodInfo>());
                }

                var codeDictionary = nameMappings[codeFolder];

                codeDictionary.Add(t.Name, startProgramMethodInfo);
            }
        }

        MethodInfo activeMethod = null;

        MethodInfo selectDemo = null;

        private MethodInfo getMethod(Type source, string methodName)
        {
            methodName = methodName.Trim();

            try
            {

                var demoMethods = source.GetRuntimeMethods();

                MethodInfo requestedMethod = null;

                foreach (MethodInfo method in demoMethods)
                {
                    if (method.Name == methodName)
                    {
                        requestedMethod = method;
                        break;
                    }
                }

                return requestedMethod;
            }
            catch
            {
                return null;
            }
        }

        private MethodInfo getMethod(string sourceName, string methodName)
        {

            Type source = Type.GetType(sourceName);

            return getMethod(source, methodName);
        }

        public async void RunSnapsApp(string requestedAppName)
        {
            MethodInfo selectedDemoMethod;
            string simpleAppName;

            if (requestedAppName.Contains(","))
            {
                // fully qualified type name - just display the name of the class
                simpleAppName = requestedAppName.Remove(requestedAppName.IndexOf(','));
            }
            else
                simpleAppName = requestedAppName;

            selectedDemoMethod = getMethod(requestedAppName, "StartProgram");

            if (selectedDemoMethod == null)
            {
                DisplayDialog("App " + simpleAppName + " not found");
                await InvokeOnUIThread(
                    async () =>
                    {
                        controlPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        await FadeElements.FadeElementOpacityAsync(controlPanel, 0, 1, new TimeSpan(0, 0, 1));
                    }
                );
            }
            else
            {
                clearDisplay();
                DisplayString("Running: " + simpleAppName);
                Delay(1);
                // Make this the active method so that it runs if we select run again
                activeMethod = selectedDemoMethod;
                StartProgram(selectedDemoMethod);
            }
        }

        string STORED_APP_NAME = "AppName";


        public void RunShortnameApp(string requestedDemoName)
        {
            string name = programType.AssemblyQualifiedName;

            name = name.Remove(0, name.IndexOf(','));

            string AssemblyQualifiedName = requestedDemoName + name;

            SaveStringToLocalStorage(STORED_APP_NAME, AssemblyQualifiedName);

            RunSnapsApp(AssemblyQualifiedName);
        }

        #endregion

#pragma warning disable 1998, 4014

        // Public start program entry point. Starts the
        // running the default user method MyProgram.StartProgram


        private Type programType;

        public async void StartProgram(Type startProgramType, string startProgramMethodName)
        {
            // Search for a special demo start method

            programType = startProgramType;

            // Find the normal start method
            activeMethod = getMethod(startProgramType, startProgramMethodName);

            StartProgram(activeMethod);
        }

        public async void StopProgram()
        {
            programCancellationSource.Cancel();
        }

        public async void RepeatProgram()
        {
            StartProgram(activeMethod);
        }

        public void RunSelectedApp(string appname)
        {
            ThreadPool.RunAsync(delegate { RunShortnameApp(appname); });
        }

        public void RepeatSnapApp()
        {
            string appName = FetchStringFromLocalStorage(STORED_APP_NAME);
            if (appName != null)
                ThreadPool.RunAsync(delegate { RunSnapsApp(appName); });
        }

        // Internal run method that starts either the 
        // default program or a demo program if one has been selected
        //

        Task programTask;

        System.Threading.CancellationTokenSource programCancellationSource = new System.Threading.CancellationTokenSource();

        async void StartProgram(MethodInfo methodToStart)
        {
            programTask = Task.Factory.StartNew(
                    async () =>
                    {
                        clearDisplay();

                        if (methodToStart == null)
                            DisplayDialog("No StartProgram method found.");
                        else
                        {
                            Object instance = Activator.CreateInstance(methodToStart.DeclaringType);
                            methodToStart.Invoke(instance, null);
                        }
                        await InvokeOnUIThread(
                            async () =>
                            {
                                if (joyPad.Visibility == Windows.UI.Xaml.Visibility.Visible)
                                {
                                    await FadeElements.FadeElementOpacityAsync(joyPad, 1, 0, new TimeSpan(0, 0, 1));
                                    joyPad.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                                    joyPad.Opacity = 1;
                                }
                                if (SnapsEngine.DisplayControMenuAtProgramEnd)
                                {
                                    controlPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;
                                    await FadeElements.FadeElementOpacityAsync(controlPanel, 0, 1, new TimeSpan(0, 0, 1));
                                }
                            }
                        );
                    },
                    programCancellationSource.Token
                    );
        }
    }
}
