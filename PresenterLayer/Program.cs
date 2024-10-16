using InfrastructureLayer.InventoryRepository;
using PresenterLayer.Helper;
using PresenterLayer.Services.Inventory;
using PresenterLayer.Views.Inventory.Inventory_Transfer_Request;
using PresenterLayer.Views.Main;
using PresenterLayer.Views.Security;
using System;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;
namespace PresenterLayer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Configure Serilog with File Sink
               
            IUnityContainer unityContainer;
            unityContainer = new UnityContainer().
                RegisterType<IInventoryTransferRequestService, InventoryTransferRequestService>(new ContainerControlledLifetimeManager()).
                RegisterType<IFrmInventoryTransferRequest, FrmInventoryTransferRequest>(new ContainerControlledLifetimeManager()).
                //RegisterType<IFrmSearch, FrmSearch>(new ContainerControlledLifetimeManager()).
                //RegisterType<ISearchService, SearchService>(new ContainerControlledLifetimeManager()).
                RegisterType<IQueryRepository, QueryRepository>(new ContainerControlledLifetimeManager());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm mainForm = new MainForm();
            StaticHelper._MainForm = mainForm;

            //IMainForm mainForm = mainPresenter.GetMainForm();
            //IMainPresenter mainPresenter = unityContainer.Resolve<MainPresenter>();
            //IInventoryTransferRequestService inventoryRequestService = unityContainer.Resolve<InventoryTransferRequestService>();
            //IFrmInventoryTransferRequest frmITR = inventoryRequestService.GetFrmITR();

            Application.Run(mainForm);
        }
    }
}
