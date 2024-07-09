using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace ClearingMetaDataPDF
{
    internal class MetaDataCleaner
    {
        public void RemoveMetadata(string PathFile)
        {
            try
            {
                using (var inputDocument = PdfReader.Open(PathFile, PdfDocumentOpenMode.Import))
                {
                    var outputDocument = new PdfDocument();

                    foreach (var page in inputDocument.Pages)
                    {
                        outputDocument.AddPage(page);
                    }

                    // обнуление метадаты
                    outputDocument.Info.Author = null;
                    outputDocument.Info.Creator = null;
                    outputDocument.Info.Keywords = null;
                    outputDocument.Info.Subject = null;
                    outputDocument.Info.Title = null;

                    //сохранение готового варианта
                    outputDocument.Save(PathFile);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

        }
    }
}
