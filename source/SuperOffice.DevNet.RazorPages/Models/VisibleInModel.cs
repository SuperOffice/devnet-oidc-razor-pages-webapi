using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Models
{
	public enum Navigation : short
	{
		/// <summary>
		/// 0: This application has no explicit navigation in GUI
		/// </summary>
		Invisible = 0,

		/// <summary>
		/// 1: This application appears in the list of the Toolbox icon in the CRM5 Navigator sidebar
		/// </summary>
		ToolboxMenu = 1,

		/// <summary>
		/// 2: This application has its own navigator button (remember to set icon resource)
		/// </summary>
		NavigatorButton = 2,

		/// <summary>
		/// 3: This application appears in the View pulldown menu
		/// </summary>
		
		ViewMenu = 3,

		/// <summary>
		/// 4: (not yet implemented) This application appears as a Task in the Selection Task card
		/// </summary>
		
		SelectionTaskCard = 4,

		/// <summary>
		/// 5: This application (type IntegratedHTML or IntegratedURL) appears as a tab on the Contact card
		/// </summary>
		
		ContactCard = 5,

		/// <summary>
		/// 6: This application (type IntegratedHTML or IntegratedURL)  appears as a tab on the Contact Archive
		/// </summary>
		
		ContactArchive = 6,

		/// <summary>
		/// 7: This application (type IntegratedHTML or IntegratedURL)  appears as a tab on the Project card
		/// </summary>
		
		ProjectCard = 7,

		/// <summary>
		/// 8: This application (type IntegratedHTML or IntegratedURL)  appears as a tab on the Project Archive
		/// </summary>
		
		ProjectArchive = 8,

		/// <summary>
		/// 9: This application (type IntegratedHTML or IntegratedURL)  appears as a tab in the Sale dialog
		/// </summary>
		
		SaleCard = 9,

		/// <summary>
		/// 10: This application (type IntegratedHTML or IntegratedURL)  appears as a tab in the Person Card
		/// </summary>
		
		PersonCard = 10,

		/// <summary>
		/// 11: This application (type IntegratedHTML or IntegratedURL)  appears as a tab in the Appointment dialog
		/// </summary>
		
		ActivityDialog = 11,

		/// <summary>
		/// 12: This application (type IntegratedHTML or IntegratedURL)  appears as a tab in the Document dialog
		/// </summary>
		
		DocumentDialog = 12,

		/// <summary>
		/// 13: In the Browser panel
		/// </summary>
		
		BrowserPanel = 13,

		/// <summary>
		/// 14: Task button visible on the Contact Selection task panel
		/// </summary>
		
		ContSelectionTask = 14,

		/// <summary>
		/// 15: Task button visible on the Appointment Selection task panel
		/// </summary>
		
		AppntSelectionTask = 15,

		/// <summary>
		/// 16: Task button visible on the Sale Selection task panel
		/// </summary>
		
		SaleSelectionTask = 16,

		/// <summary>
		/// 17: Task button visible on the Document Selection task panel
		/// </summary>
		
		DocSelectionTask = 17,

		/// <summary>
		/// 18: Task button visible on the Project Selection task panel
		/// </summary>
		
		ProjSelectionTask = 18,

		/// <summary>
		/// 19: In company minicard
		/// </summary>
		
		CompanyMinicard = 19,

		/// <summary>
		/// 20: In project minicard
		/// </summary>
		
		ProjectMinicard = 20,

		/// <summary>
		/// 21: In diary minicard
		/// </summary>
		
		DiaryMinicard = 21,

		/// <summary>
		/// 22: In selection minicard
		/// </summary>
		
		SelectionMinicard = 22,

		/// <summary>
		/// 23: In the main ButtonBar
		/// </summary>
		
		ButtonPanelTask = 23,

		/// <summary>
		/// 24: In the appointment dialog
		/// </summary>
		
		AppointmentDialogTask = 24,

		/// <summary>
		/// 25: In the sale dialog
		/// </summary>
		
		SaleDialogTask = 25,

		/// <summary>
		/// 26: In the document dialog
		/// </summary>
		
		DocumentDialogTask = 26,

		/// <summary>
		/// 27: In the person dialog
		/// </summary>
		
		PersonDialogTask = 27,

		/// <summary>
		/// 28: In the sale minicard
		/// </summary>
		
		SaleMinicard = 28,

		/// <summary>
		/// 29: In the sale archive
		/// </summary>
		
		SaleArchive = 29,

		/// <summary>
		/// 30: Task tab for appointment selection, while showing shadow sel
		/// </summary>
		
		AppntSelectionShadowTask = 30,

		/// <summary>
		/// 31: Task tab for sale selection, while showing shadow sel
		/// </summary>
		
		SaleSelectionShadowTask = 31,

		/// <summary>
		/// 32: Task tab for document selection, while showing shadow sel
		/// </summary>
		
		DocSelectionShadowTask = 32,

		/// <summary>
		/// 33: Task tab for project selection, while showing shadow sel
		/// </summary>
		
		ProjSelectionShadowTask = 33,

		/// <summary>
		/// 34: Context (popup) menu in Diary archive
		/// </summary>
		
		DiaryArchive = 34,

		/// <summary>
		/// 35: Context (popup) menu in Contact selection
		/// </summary>
		
		SelectionContactArchive = 35,

		/// <summary>
		/// 36: Context (popup) menu in Project archive
		/// </summary>
		
		SelectionProjectArchive = 36,

		/// <summary>
		/// 37: Context (popup) menu in Sale archive
		/// </summary>
		
		SelectionSaleArchive = 37,

		/// <summary>
		/// 38: Context (popup) menu in Appointment archive
		/// </summary>
		
		SelectionAppointmentArchive = 38,

		/// <summary>
		/// 39: Context (popup) menu in Document archive
		/// </summary>
		
		SelectionDocumentArchive = 39,

		/// <summary>
		/// 40: Task card in Contact selection, when a custom archive is shown
		/// </summary>
		
		ContSelectionCustomTask = 40,

		/// <summary>
		/// 41: Task card in Appointment selection, when a custom archive is shown
		/// </summary>
		
		AppntSelectionCustomTask = 41,

		/// <summary>
		/// 42: Task card in Sale selection, when a custom archive is shown
		/// </summary>
		
		SaleSelectionCustomTask = 42,

		/// <summary>
		/// 43: Task card in Document selection, when a custom archive is shown
		/// </summary>
		
		DocSelectionCustomTask = 43,

		/// <summary>
		/// 44: Task card in Project selection, when a custom archive is shown
		/// </summary>
		
		ProjSelectionCustomTask = 44,

		/// <summary>
		/// 45: ?
		/// </summary>
		
		CustomArchiveMiniCard = 45,

		/// <summary>
		/// 46: ?
		/// </summary>
		
		SelectionCard = 46,

		/// <summary>
		/// 47: In the Reporter panel minicard, so far only in Web
		/// </summary>
		
		ReportMinicard = 47,

		/// <summary>
		/// 48: 
		/// </summary>
		
		QuoteDialog = 48,

		/// <summary>
		/// 49: 
		/// </summary>
		
		QuoteDialogTask = 49,

		/// <summary>
		/// 50: 
		/// </summary>
		
		QuoteDialogArchive = 50,

		/// <summary>
		/// 51: 
		/// </summary>
		
		QuoteLineDialogTask = 51,

		/// <summary>
		/// 52: 
		/// </summary>
		
		QuoteLineDialog = 52,

		/// <summary>
		/// 53: 
		/// </summary>
		
		QuoteLineSelectionMainTask = 53,

		/// <summary>
		/// 54: 
		/// </summary>
		
		QuoteLineSelectionShadowTask = 54,

		/// <summary>
		/// 55: 
		/// </summary>
		
		SelectionQuoteLineArchive = 55,

		/// <summary>
		/// 56: 
		/// </summary>
		
		QuoteLineSelectionCustomTask = 56,

		/// <summary>
		/// 57: 
		/// </summary>
		
		FindSystem = 57,

		/// <summary>
		/// 58: Task button visible on the Mailing Selection task panel
		/// </summary>
		
		MailingSelectionTask = 58,

		/// <summary>
		/// 59: Task button visible on the Mailing Selection task panel
		/// </summary>
		
		ContactSelectionMailingsTask = 59,

		/// <summary>
		/// 60: Task button visible on the Mailing Selection task panel
		/// </summary>
		
		AppointmentSelectionMailingsTask = 60,

		/// <summary>
		/// 61: Task button visible on the Mailing Selection task panel
		/// </summary>
		
		SaleSelectionMailingsTask = 61,

		/// <summary>
		/// 62: Task button visible on the Mailing Selection task panel
		/// </summary>
		
		DocumentSelectionMailingsTask = 62,

		/// <summary>
		/// 63: Task button visible on the Mailing Selection task panel
		/// </summary>
		
		ProjectSelectionMailingsTask = 63,

		/// <summary>
		/// 64: Task button visible on the Mailing Selection task panel
		/// </summary>
		
		QuoteLineSelectionMailingsTask = 64,

		/// <summary>
		/// 65: Visible in top panels new menu
		/// </summary>
		
		TopPanelNewMenu = 65,

		/// <summary>
		/// 66: Dashboard panel
		/// </summary>
		
		Dashboard = 66,

		/// <summary>
		/// 67: This application (type IntegratedHTML or IntegratedURL)  appears as a tab in the Person Archive
		/// </summary>
		
		PersonArchive = 67,

		/// <summary>
		/// 68: In the person minicard
		/// </summary>
		
		PersonMinicard = 68,

	}
}
