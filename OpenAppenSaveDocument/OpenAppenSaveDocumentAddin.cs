using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using System;
using System.IO;

namespace OpenAppenSaveDocument
{
    [Plugin("OpenAppendSave", "ITI", DisplayName = "OpenAppendSave")]
    [AddInPlugin(AddInLocation.Export)]
	public class OpenAppenSaveDocumentAddin : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {
			const string sourceDir = @"E:\Work\Freelance\ITI\2022-ICC-BIMAD-2\01-Navisworks\Session 04\Source Files\Practice Source Files\2021";
			const string targetDir = @"E:\Work\Freelance\ITI\2022-ICC-BIMAD-2\01-Navisworks\Session 04\Source Files\Practice Source Files\2021\Navis\NWF";
			const string nwcSearchPattern = "*.nwc";
			const string nwfFileExtension = ".nwf";
			const string nwdFileExtension = ".nwd";
			const string documentPassword = "123";
			const string documentTitle = "123";

			var oDocument = Application.ActiveDocument;

            if (!oDocument.IsClear)
            {
				oDocument.Clear();
			}

            var targetFileName = Path.Combine(targetDir, DateTime.Now.ToString("yyyyMMdd_hhmmss") + "-MyDocument");

			string[] files = Directory.GetFiles(sourceDir, nwcSearchPattern);
			var isSourceDirExist = File.Exists(sourceDir);
			if (isSourceDirExist)
			{
				System.Windows.Forms.MessageBox.Show("Source Dir Cannot be found!");
			}

			try
			{
				oDocument.AppendFiles(files);
			}
			catch (DocumentFileException)
			{
				var isFilesAppended = oDocument.TryAppendFiles(files);
				if (!isFilesAppended)
				{
					System.Windows.Forms.MessageBox.Show("Cannot Append Files");
				}
			}
			catch (Exception exp)
			{
				System.Windows.Forms.MessageBox.Show($"Unhandled Exception: {exp.Message}");
			}

			try
			{
				oDocument.SaveFile(targetFileName + nwfFileExtension, DocumentFileVersion.Navisworks2021);
			}
			catch (DocumentFileException)
			{
				var isFileSaved = oDocument.TrySaveFile(targetFileName + nwfFileExtension, DocumentFileVersion.Navisworks2021);

				if (!isFileSaved)
				{
					System.Windows.Forms.MessageBox.Show($"Cannot Save NWF File, at path: {targetFileName}");
				}
			}
			catch (InvalidOperationException)
			{
				System.Windows.Forms.MessageBox.Show($"Document is Clear and has no files");
			}
			catch (Exception exp)
			{
				System.Windows.Forms.MessageBox.Show($"Unhandled Exception: {exp.Message}");
			}


			var publishSettings = new PublishProperties();
			publishSettings.Title = documentTitle;
			publishSettings.ExpiryDate = DateTime.Now.AddDays(1);
			publishSettings.SetPassword(documentPassword);

			try
			{
				oDocument.PublishFile(targetFileName+ nwdFileExtension, publishSettings);
			}
			catch (DocumentFileException)
			{
				var isPublished = oDocument.TryPublishFile(targetFileName+ nwdFileExtension, publishSettings);
				if (!isPublished)
				{
					System.Windows.Forms.MessageBox.Show($"Cannot publish NWD File, at path: {targetFileName}");
				}
			}

			return 0;
        }
    }
}
