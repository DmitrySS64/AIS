using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using word = Microsoft.Office.Interop.Word;


namespace Lab_9
{
    internal class COMFormatter
    {
        private word.Application wordapp = new word.Application();
        private word.Document worddocument;
        private word.Documents worddocuments;
        private string outputPath = @"D:\мусор\newDoc.doc";

        public COMFormatter(string template = @"D:\a2.doc")
        {
            wordapp.Visible = true;

            Object newTemplate = false;
            Object documentType = word.WdNewDocumentType.wdNewBlankDocument;
            Object visible = true;

            //wordapp.Document.Add(template, newTemplate, ref documentType, ref visible);

            //worddocuments = wordapp.Documents;
            //worddocument = worddocuments.getItem(1);
            //worddocument.Active();

            // Открываем документ на основе шаблона
            worddocuments = wordapp.Documents;
            worddocument = worddocuments.Open(template, ref newTemplate, ref documentType, ref visible);
            worddocument.Activate();
        }
        public void Replace(string wordr, string replacement)
        {
            word.Range range = worddocument.StoryRanges[word.WdStoryType.wdMainTextStory];
            range.Find.ClearFormatting();

            range.Find.Execute(FindText: wordr, ReplaceWith: replacement);

            TrySave();
        }
        public void ReplaceBookmark(string bookmark, string replacement)
        {
            worddocument.Bookmarks[bookmark].Range.Text = replacement;

            TrySave();
        }

        public void TrySave()
        {
            try
            {
                worddocument.SaveAs(outputPath, word.WdSaveFormat.wdFormatDocument);
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.Message);
            }
        }
        public void Close()
        {
            Object saveChanges = word.WdSaveOptions.wdPromptToSaveChanges;
            Object originalFormat = word.WdOriginalFormat.wdWordDocument;
            Object routeDocument = Type.Missing;
            wordapp.Quit(ref saveChanges, ref originalFormat, ref routeDocument);
        }
    }
}
