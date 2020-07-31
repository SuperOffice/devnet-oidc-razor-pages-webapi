using System.Runtime.Serialization;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Models.Enums
{
    public enum VisibleInModel
    {
        [EnumMember(Value = @"Invisible")]
        Invisible = 0,

        [EnumMember(Value = @"ToolboxMenu")]
        ToolboxMenu = 1,

        [EnumMember(Value = @"NavigatorButton")]
        NavigatorButton = 2,

        [EnumMember(Value = @"ViewMenu")]
        ViewMenu = 3,

        [EnumMember(Value = @"SelectionTaskCard")]
        SelectionTaskCard = 4,

        [EnumMember(Value = @"ContactCard")]
        ContactCard = 5,

        [EnumMember(Value = @"ContactArchive")]
        ContactArchive = 6,

        [EnumMember(Value = @"ProjectCard")]
        ProjectCard = 7,

        [EnumMember(Value = @"ProjectArchive")]
        ProjectArchive = 8,

        [EnumMember(Value = @"SaleCard")]
        SaleCard = 9,

        [EnumMember(Value = @"PersonCard")]
        PersonCard = 10,

        [EnumMember(Value = @"ActivityDialog")]
        ActivityDialog = 11,

        [EnumMember(Value = @"DocumentDialog")]
        DocumentDialog = 12,

        [EnumMember(Value = @"BrowserPanel")]
        BrowserPanel = 13,

        [EnumMember(Value = @"ContSelectionTask")]
        ContSelectionTask = 14,

        [EnumMember(Value = @"AppntSelectionTask")]
        AppntSelectionTask = 15,

        [EnumMember(Value = @"SaleSelectionTask")]
        SaleSelectionTask = 16,

        [EnumMember(Value = @"DocSelectionTask")]
        DocSelectionTask = 17,

        [EnumMember(Value = @"ProjSelectionTask")]
        ProjSelectionTask = 18,

        [EnumMember(Value = @"CompanyMinicard")]
        CompanyMinicard = 19,

        [EnumMember(Value = @"ProjectMinicard")]
        ProjectMinicard = 20,

        [EnumMember(Value = @"DiaryMinicard")]
        DiaryMinicard = 21,

        [EnumMember(Value = @"SelectionMinicard")]
        SelectionMinicard = 22,

        [EnumMember(Value = @"ButtonPanelTask")]
        ButtonPanelTask = 23,

        [EnumMember(Value = @"AppointmentDialogTask")]
        AppointmentDialogTask = 24,

        [EnumMember(Value = @"SaleDialogTask")]
        SaleDialogTask = 25,

        [EnumMember(Value = @"DocumentDialogTask")]
        DocumentDialogTask = 26,

        [EnumMember(Value = @"PersonDialogTask")]
        PersonDialogTask = 27,

        [EnumMember(Value = @"SaleMinicard")]
        SaleMinicard = 28,

        [EnumMember(Value = @"SaleArchive")]
        SaleArchive = 29,

        [EnumMember(Value = @"AppntSelectionShadowTask")]
        AppntSelectionShadowTask = 30,

        [EnumMember(Value = @"SaleSelectionShadowTask")]
        SaleSelectionShadowTask = 31,

        [EnumMember(Value = @"DocSelectionShadowTask")]
        DocSelectionShadowTask = 32,

        [EnumMember(Value = @"ProjSelectionShadowTask")]
        ProjSelectionShadowTask = 33,

        [EnumMember(Value = @"DiaryArchive")]
        DiaryArchive = 34,

        [EnumMember(Value = @"SelectionContactArchive")]
        SelectionContactArchive = 35,

        [EnumMember(Value = @"SelectionProjectArchive")]
        SelectionProjectArchive = 36,

        [EnumMember(Value = @"SelectionSaleArchive")]
        SelectionSaleArchive = 37,

        [EnumMember(Value = @"SelectionAppointmentArchive")]
        SelectionAppointmentArchive = 38,

        [EnumMember(Value = @"SelectionDocumentArchive")]
        SelectionDocumentArchive = 39,

        [EnumMember(Value = @"ContSelectionCustomTask")]
        ContSelectionCustomTask = 40,

        [EnumMember(Value = @"AppntSelectionCustomTask")]
        AppntSelectionCustomTask = 41,

        [EnumMember(Value = @"SaleSelectionCustomTask")]
        SaleSelectionCustomTask = 42,

        [EnumMember(Value = @"DocSelectionCustomTask")]
        DocSelectionCustomTask = 43,

        [EnumMember(Value = @"ProjSelectionCustomTask")]
        ProjSelectionCustomTask = 44,

        [EnumMember(Value = @"CustomArchiveMiniCard")]
        CustomArchiveMiniCard = 45,

        [EnumMember(Value = @"SelectionCard")]
        SelectionCard = 46,

        [EnumMember(Value = @"ReportMinicard")]
        ReportMinicard = 47,

        [EnumMember(Value = @"QuoteDialog")]
        QuoteDialog = 48,

        [EnumMember(Value = @"QuoteDialogTask")]
        QuoteDialogTask = 49,

        [EnumMember(Value = @"QuoteDialogArchive")]
        QuoteDialogArchive = 50,

        [EnumMember(Value = @"QuoteLineDialogTask")]
        QuoteLineDialogTask = 51,

        [EnumMember(Value = @"QuoteLineDialog")]
        QuoteLineDialog = 52,

        [EnumMember(Value = @"QuoteLineSelectionMainTask")]
        QuoteLineSelectionMainTask = 53,

        [EnumMember(Value = @"QuoteLineSelectionShadowTask")]
        QuoteLineSelectionShadowTask = 54,

        [EnumMember(Value = @"SelectionQuoteLineArchive")]
        SelectionQuoteLineArchive = 55,

        [EnumMember(Value = @"QuoteLineSelectionCustomTask")]
        QuoteLineSelectionCustomTask = 56,

        [EnumMember(Value = @"FindSystem")]
        FindSystem = 57,

        [EnumMember(Value = @"MailingSelectionTask")]
        MailingSelectionTask = 58,

        [EnumMember(Value = @"ContactSelectionMailingsTask")]
        ContactSelectionMailingsTask = 59,

        [EnumMember(Value = @"AppointmentSelectionMailingsTask")]
        AppointmentSelectionMailingsTask = 60,

        [EnumMember(Value = @"SaleSelectionMailingsTask")]
        SaleSelectionMailingsTask = 61,

        [EnumMember(Value = @"DocumentSelectionMailingsTask")]
        DocumentSelectionMailingsTask = 62,

        [EnumMember(Value = @"ProjectSelectionMailingsTask")]
        ProjectSelectionMailingsTask = 63,

        [EnumMember(Value = @"QuoteLineSelectionMailingsTask")]
        QuoteLineSelectionMailingsTask = 64,

        [EnumMember(Value = @"TopPanelNewMenu")]
        TopPanelNewMenu = 65,

        [EnumMember(Value = @"Dashboard")]
        Dashboard = 66,

        [EnumMember(Value = @"PersonArchive")]
        PersonArchive = 67,

        [EnumMember(Value = @"PersonMinicard")]
        PersonMinicard = 68,

    }
}