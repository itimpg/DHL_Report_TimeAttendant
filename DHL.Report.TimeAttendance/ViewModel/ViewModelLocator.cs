using AutoMapper;
using DHL.Report.TimeAttendance.Data;
using DHL.Report.TimeAttendance.Data.Entities;
using DHL.Report.TimeAttendance.Managers;
using DHL.Report.TimeAttendance.Managers.Interfaces;
using DHL.Report.TimeAttendance.Models;
using DHL.Report.TimeAttendance.Repositories;
using DHL.Report.TimeAttendance.Repositories.Interfaces;
using DHL.Report.TimeAttendance.Services;
using DHL.Report.TimeAttendance.Services.Interfaces;
using DHL.Report.TimeAttendance.TypeConverters;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.IO;

namespace DHL.Report.TimeAttendance.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<TimeSpan, string>().ConvertUsing(new StringToTimeSpanTypeConverter());
                cfg.CreateMap<string, TimeSpan>().ConvertUsing(new TimeSpanToStringTypeConverter());
                cfg.CreateMap<Shift, ShiftModel>().ReverseMap();
            });

            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<IXmlManager, XmlManager>();
            SimpleIoc.Default.Register<IAboutManager, AboutManager>();

            SimpleIoc.Default.Register<IExcelDataManager, ExcelDataManager>();
            SimpleIoc.Default.Register<IAccessDataManager, AccessDataManager>();
            SimpleIoc.Default.Register<IExcelReportManager, ExcelReportManager>();

            SimpleIoc.Default.Register<IMyContext>(() =>
            {
                string connectionString = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, "dhl_timeattencance_report.db");
                return new MyContext(connectionString);
            });

            SimpleIoc.Default.Register<IShiftRepository>(() =>
            {
                return new ShiftRepository(
                    SimpleIoc.Default.GetInstance<IMyContext>());
            });

            SimpleIoc.Default.Register<IShiftManager>(() =>
            {
                return new ShiftManager(
                    SimpleIoc.Default.GetInstance<IShiftRepository>());
            });

            SimpleIoc.Default.Register<IConfigManager>(() =>
            {
                string appPath = AppDomain.CurrentDomain.BaseDirectory;

                return new ConfigManager(
                    SimpleIoc.Default.GetInstance<IXmlManager>(),
                    appPath);
            });

            SimpleIoc.Default.Register<IReportManager>(() =>
            {
                string appPath = AppDomain.CurrentDomain.BaseDirectory;

                return new ReportManager(
                    SimpleIoc.Default.GetInstance<IExcelDataManager>(),
                    SimpleIoc.Default.GetInstance<IAccessDataManager>(),
                    SimpleIoc.Default.GetInstance<IShiftManager>(),
                    SimpleIoc.Default.GetInstance<IExcelReportManager>());
            });

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SettingViewModel>();
            SimpleIoc.Default.Register<AboutViewModel>();
            SimpleIoc.Default.Register<ShiftViewModel>();
        }

        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        public SettingViewModel Setting
        {
            get { return ServiceLocator.Current.GetInstance<SettingViewModel>(); }
        }

        public AboutViewModel About
        {
            get { return ServiceLocator.Current.GetInstance<AboutViewModel>(); }
        }

        public ShiftViewModel Shift
        {
            get { return ServiceLocator.Current.GetInstance<ShiftViewModel>(); }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}