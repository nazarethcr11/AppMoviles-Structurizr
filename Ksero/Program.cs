using Structurizr;
using Structurizr.Api;

namespace c4_model_design{

    class Program
    {
        static void Main(string[] args)
        {
            Banking();
        }

        static void Banking()
        {
            const long workspaceId = 70049;
            const string apiKey = "474cb9f4-9fe0-40ef-ad82-2c4feea4ceef";
            const string apiSecret = "507f0f6b-18ff-46fb-86b6-693dc8cfac03";

            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);
            //nombre
            Workspace workspace = new Workspace("Jobit", "Sistema intermediario de empleadores-trabajadores");
            ViewSet viewSet = workspace.Views;
            Model model = workspace.Model;

            // 1. Diagrama de Contexto

            //sistemas que usas
            SoftwareSystem gmail = model.AddSoftwareSystem("GoogleEmail", "Plataforma que ofrece una REST API de correo.");

            //sistema propio
            SoftwareSystem Jobit = model.AddSoftwareSystem("Jobit App", "Aplicacion para encontrar de trabajo");

            //personas
            Person adminUser = model.AddPerson("Administator", "Usuario que administra la app");
            Person employeer = model.AddPerson("Employer", "Usuario que ofrece trabajo / busca empleados");
            Person employee = model.AddPerson("Employee", "Usuario busca trabajo ");
            
           //relacion de uso --- personas
           
            adminUser.Uses(Jobit, "Administra la aplicacion");
            employeer.Uses(Jobit, "Crea/Modifica ofertas de trabajo");
            employee.Uses(Jobit, "Aplica a las ofertas de trabajo");
           
            //lo que tu app usa (sistemas externos que tu app usa)
          
            Jobit.Uses(gmail, "Usa la API de Gmail ");


            // Tags

            adminUser.AddTags("Usuario que administra");
            employeer.AddTags("Usurio que modifica  ofertas de trabajo");
            employee.AddTags("Usuario que aplica a los trabajos");
            
            Jobit.AddTags("Jobit App");
 
            gmail.AddTags("GoogleEmail");


            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle("Usuario que administra")
                {Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person});
            styles.Add(new ElementStyle("Usurio que modifica  ofertas de trabajo")
                {Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person});
            styles.Add(new ElementStyle("Usuario que aplica a los trabajos")
                {Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person});
            styles.Add(new ElementStyle("Jobit App")
                {Background = "#008f39", Color = "#ffffff", Shape = Shape.RoundedBox});
            styles.Add(new ElementStyle("GoogleEmail")
                {Background = "#2f95c7", Color = "#ffffff", Shape = Shape.RoundedBox});

            //code para que se muestre en e structurizr
            SystemContextView contextView = viewSet.CreateSystemContextView(Jobit, "Contexto", "Diagrama de contexto");
            contextView.PaperSize = PaperSize.A4_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            
            // 2. Diagrama de Contenedores
            
            //lo basico
            Container mobileApplication = Jobit.AddContainer("Mobile App", "", "Kotlin");
            Container landingPage = Jobit.AddContainer("Landing Page", "", "Angular");
            Container apiRest = Jobit.AddContainer("API Rest", "API Rest", "NodeJS (NestJS) port 8080");
            Container dataBase = Jobit.AddContainer("DataBase", "", "MySQL");
            
            
            //bounded context
            
            Container employeeContext = Jobit.AddContainer("RetailerContext", "Contexto de los empleados");
            Container employerContext = Jobit.AddContainer("WholeSalerContext", "Contexto de los empleadores");
            Container notifyContext = Jobit.AddContainer("NotifyContext", "Contexto de notificacion a correos");
            Container securityContext = Jobit.AddContainer("SecurityContext", "Contexto de validacion de usuarios");
            Container meetingRecordContext = Jobit.AddContainer("MeetingRecordContext", "Contexto de reuniones acordados ");
            
            //others relations
            mobileApplication.Uses(apiRest, "API Request", "JSON/HTTPS");
            landingPage.Uses(apiRest, "API Request", "JSON/HTTPS");
            
            //usuarios uses
            //mobile app
            adminUser.Uses(mobileApplication, "", "");
            employeer.Uses(mobileApplication, "", "");
            employee.Uses(mobileApplication, "", "");

            //landing page
            adminUser.Uses(landingPage, "", "");
            employeer.Uses(landingPage, "", "");
            employee.Uses(landingPage, "", "");
            
            
            //Api uses BC
            
            apiRest.Uses(employeeContext, "", "");
            apiRest.Uses(employerContext, "", "");
            apiRest.Uses(notifyContext, "", "");
            apiRest.Uses(meetingRecordContext, "", "");
            apiRest.Uses(securityContext, "", "");
            //database
            employerContext.Uses(dataBase, "", "JDBC");
            employeeContext.Uses(dataBase, "", "JDBC");
            notifyContext.Uses(dataBase, "", "JDBC");
            meetingRecordContext.Uses(dataBase, "", "JDBC");
            securityContext.Uses(dataBase, "", "JDBC");
            
            //app use aditionals

            notifyContext.Uses(gmail, "API Request", "JSON/HTTPS");


            // Tags
            mobileApplication.AddTags("MobileApp");
            landingPage.AddTags("LandingPage");
            apiRest.AddTags("APIRest");
            dataBase.AddTags("Database");
            
            employerContext.AddTags("EmployerContext");
            employeeContext.AddTags("EmployeeContext");
            notifyContext.AddTags("NotifyContext");
            meetingRecordContext.AddTags("MeetingContext");
            securityContext.AddTags("SecurityContext");

            styles.Add(new ElementStyle("MobileApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.MobileDevicePortrait, Icon = "" });
            styles.Add(new ElementStyle("WebApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("LandingPage") { Background = "#929000", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("APIRest") { Shape = Shape.RoundedBox, Background = "#0000ff", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("Database") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });
            
            styles.Add(new ElementStyle("EmployerContext") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("EmployeeContext") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("NotifyContext") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MeetingContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("SecurityContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            
            

            ContainerView containerView = viewSet.CreateContainerView(Jobit, "Contenedor", "Diagrama de contenedores");
            contextView.PaperSize = PaperSize.A4_Landscape;
            containerView.AddAllElements();
            
             //NO USAR//
            //Components
            //notify context
            Component emailComponent = apiRest.AddComponent("EmailComponent", "Envia mensaje a los usuarios");
            //EmployerContext 
            Component requestSummaryComponent = apiRest.AddComponent("RequestSummaryComponent", "Provee el registro de solicitudes a un trabajo ");
            Component accountEmployerController = apiRest.AddComponent("AccountEmployerController", "Modifica informacion de la propia cuenta(empleador)");
            Component modifyComponent = apiRest.AddComponent("ModifyComponent", "Modifica informacion de sus solicitudes");
            Component aproveComponent = apiRest.AddComponent("AproveComponent", "Administra la aprobacion de empleados");
            
            //EmployeeContext
            Component applyComponent = apiRest.AddComponent("ApplyComponent", "Administra el proceso de aplicar a un trabajo");
            Component accountEmployeeController = apiRest.AddComponent("AccountEmployeeController", "Modifica informacion de la propia cuenta(empleado)");
            //Security context
            Component securityComponent = apiRest.AddComponent("SecurityComponent", "Provee funcionalidad relacionada a la verificacion de pagos y usuarios");
            Component authorityController = apiRest.AddComponent("AuthorityController", "Realiza y verifica las identidades de los usuarios");
           //MeetingRecordContext 
           Component interviewRecordSummaryController = apiRest.AddComponent("SaleRecordSummaryController", "Provee el registro de entrevistas realizados");
           Component meetingDetailComponent = apiRest.AddComponent("OrderDetailComponent", "Muestra el detalle de la reunion");
            
           //USES
           mobileApplication.Uses(accountEmployeeController,"","");
           mobileApplication.Uses(authorityController,"","");
           mobileApplication.Uses(interviewRecordSummaryController,"","");
           mobileApplication.Uses(accountEmployerController,"","");
           
           //interview uses
           interviewRecordSummaryController.Uses(meetingDetailComponent, "", "");
          //  uses
          accountEmployeeController.Uses(applyComponent, "", "");
          // uses
          accountEmployerController.Uses(requestSummaryComponent, "", "");
          accountEmployerController.Uses(modifyComponent, "", "");
          accountEmployerController.Uses(aproveComponent, "", "");
          
          //payController
          authorityController.Uses(securityComponent, "", "");
          authorityController.Uses(emailComponent, "", "");
          //others that use email
          meetingDetailComponent.Uses(emailComponent, "", "");
           
          //uses database
          securityComponent.Uses(dataBase, "", "JDBC");
          meetingDetailComponent.Uses(dataBase, "", "JDBC");
          
          //others
          emailComponent.Uses(gmail, "", "");

          //tags
          
          emailComponent.AddTags("EmailComponent");
          requestSummaryComponent.AddTags("RequestSummaryComponent");
          accountEmployerController.AddTags("AccountEmployerController");
          modifyComponent.AddTags("ModifyComponent");
          aproveComponent.AddTags("AproveComponent");
          applyComponent.AddTags("ApplyComponent");
          accountEmployeeController.AddTags("AccountEmployeeController");
          securityComponent.AddTags("SecurityComponent");
          authorityController.AddTags("AuthorityController");
          interviewRecordSummaryController.AddTags("InterviewRecordSummaryController");
          meetingDetailComponent.AddTags("MeetingDetailComponent");

        //styles
        styles.Add(new ElementStyle("EmailComponent") { Shape = Shape.Component,Color = "#ffffff", Background = "#555555", Icon = "" });
        styles.Add(new ElementStyle("RequestSummaryComponent") { Shape = Shape.Component,Color = "#ffffff", Background = "#555555", Icon = "" });
        styles.Add(new ElementStyle("AccountEmployerController") { Shape = Shape.Component,Color = "#ffffff", Background = "#555555", Icon = "" });
        styles.Add(new ElementStyle("ModifyComponent") { Shape = Shape.Component,Color = "#ffffff", Background = "#555555", Icon = "" });
        styles.Add(new ElementStyle("AproveComponent") { Shape = Shape.Component,Color = "#ffffff", Background = "#555555", Icon = "" });
        styles.Add(new ElementStyle("ApplyComponent") { Shape = Shape.Component,Color = "#ffffff", Background = "#555555", Icon = "" });
        styles.Add(new ElementStyle("AccountEmployeeController") { Shape = Shape.Component,Color = "#ffffff", Background = "#555555", Icon = "" });
        styles.Add(new ElementStyle("SecurityComponent") { Shape = Shape.Component,Color = "#ffffff", Background = "#555555", Icon = "" });
        styles.Add(new ElementStyle("AuthorityController") { Shape = Shape.Component,Color = "#ffffff", Background = "#555555", Icon = "" });
        styles.Add(new ElementStyle("InterviewRecordSummaryController") { Shape = Shape.Component,Color = "#ffffff", Background = "#555555", Icon = "" });
        styles.Add(new ElementStyle("MeetingDetailComponent") { Shape = Shape.Component,Color = "#ffffff", Background = "#555555", Icon = "" });


        ComponentView componentView = viewSet.CreateComponentView(apiRest, "Component", "Diagrama de componentes");
        componentView.PaperSize = PaperSize.A3_Landscape;
        componentView.AddAllElements();
        componentView.Remove(employerContext);
        componentView.Remove(employeeContext);
        componentView.Remove(notifyContext);
        componentView.Remove(meetingRecordContext);
        componentView.Remove(securityContext);

        //NO BORRES ESTO
            
            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
            
            
        }
    }
}