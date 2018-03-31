﻿using Prism;
using Prism.Ioc;
using PrismApp.ViewModels;
using PrismApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Autofac;
using Serilog;
using PrismApp.HistorySink;
using System;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PrismApp
{
    public partial class App : PrismApplication
    {
		private ILogHistory _logHistory = new LogHistory();

        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

			Log.Logger = new LoggerConfiguration()
				.WriteTo.HistorySink(_logHistory)
				.WriteTo.EvlSink("", "https://swamp-evl.azurewebsites.net/events", "PrismApp")
				.CreateLogger();

			Log.Information("Start");

			await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
			containerRegistry.RegisterInstance<ILogHistory>(_logHistory);

			containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
			containerRegistry.RegisterForNavigation<SpeakPage>();
			containerRegistry.RegisterForNavigation<PostApiPage>();
			containerRegistry.RegisterForNavigation<PrintLabelPage>();
			containerRegistry.RegisterForNavigation<LogHistoryPage>();
		}
    }
}
