using System.Xml;
using System;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace ExampleActions
{
    public class ExampleClass
    {
        // returns true, and so refreshes the grid from which the action was called
        public static bool Refresh()
        {
            return true;
        }

        // presents a message box with the collection identifier, alternate identifier, transaction type, and estimate route
        // if the user clicks OK, the grid will refresh
        public static bool ExampleFunction(string collectionIdentifier, string collectionAlternateIdentifier, string transactionType, int estimateRoute)
        {
            return MessageBox.Show($"You have {collectionIdentifier}{(collectionAlternateIdentifier != null ? $" ({collectionAlternateIdentifier})" : "")} open.\n\nThis button was triggered from the {transactionType} tab.\n\nThe current non-stocked code estimate route open is {estimateRoute}.",
                                    "Hello, World!", MessageBoxButtons.OKCancel) == DialogResult.OK;
        }

        // presents a list of all transactions on the collection and indicates which are selected
        public static void GetSelectedTransactions(Tuple<string, string>[] allTransactionKeys, Tuple<string, string>[] selectedTransactionKeys, string transactionType)
        {
            string message = "All transactions on the collection:";

            var allKeys = allTransactionKeys.GroupBy(x => x.Item1);
            foreach (var keySubset in allKeys)
            {
                message += $"\n\n{keySubset.Key}{(transactionType == keySubset.Key ? $" (current context)" : "")}:";
                foreach (var transaction in keySubset)
                    message += $"\n\t{transaction.Item2}{(selectedTransactionKeys.Any(x => transaction.Item1 == x.Item1 && transaction.Item2 == x.Item2) ? $" (selected)" : "")}";
            }

            MessageBox.Show(message, $"{selectedTransactionKeys.Length} / {allTransactionKeys.Length} transactions selected");
        }

        // executes a business object call and writes the results to a file
        public static void BusinessObjectFunction(Func<string, string, string, string, string, XmlDocument> businessObjectCallFunction)
        {
            XmlDocument results = ExecuteBusinessObjectCall(businessObjectCallFunction, "SORQRY", "<Query>\r\n<Key>\r\n   <SalesOrder>1</SalesOrder>\r\n   <Invoice></Invoice>\r\n</Key>\r\n<Option>\r\n    <IncludeStockedLines>Y</IncludeStockedLines>\r\n    <IncludeNonStockedLines>Y</IncludeNonStockedLines>\r\n    <IncludeFreightLines>Y</IncludeFreightLines>\r\n    <IncludeMiscLines>Y</IncludeMiscLines>\r\n    <IncludeCommentLines>Y</IncludeCommentLines>\r\n    <IncludeCompletedLines>Y</IncludeCompletedLines>\r\n    <IncludeSerials>N</IncludeSerials>\r\n    <IncludeLots>Y</IncludeLots>\r\n    <IncludeBins>Y</IncludeBins>\r\n    <IncludeAttachedItems>N</IncludeAttachedItems>\r\n    <IncludeCustomForms>Y</IncludeCustomForms>\r\n    <IncludeDetailLineCustomForms>Y</IncludeDetailLineCustomForms>\r\n    <IncludeValues>N</IncludeValues>\r\n    <ReturnLineShipDate>N</ReturnLineShipDate>\r\n    <IncludeKitComponents>Y</IncludeKitComponents>\r\n    <IncludeNotes>Y</IncludeNotes>\r\n\t<IncludePickingDetails>N</IncludePickingDetails>\r\n    <ReturnNotesinBlock>N</ReturnNotesinBlock>\r\n    <XslStylesheet/>\r\n</Option>\r\n</Query>", "", "QUERY", "QUERY");
            File.WriteAllText("C:\\ExampleBusinessObjectCallResults.xml", results.OuterXml);
        }

        // abstracts the business object call to a function with named parameters
        private static XmlDocument ExecuteBusinessObjectCall(Func<string, string, string, string, string, XmlDocument> businessObjectCallFunction, string businessObject, string xmlInput, string xmlParameters = "", string boClass = "QUERY", string boMethod = "QUERY")
        {
            return businessObjectCallFunction(businessObject, xmlInput, xmlParameters, boClass, boMethod);
        }


        // non-static members
        string constructorTextFilePath = null;
        int nonStaticMember = 0;

        // constructor to be used for non-static functions
        public ExampleClass(string collectionIdentifier)
        {
            constructorTextFilePath = "C:\\ExampleClassConstructor.txt";
            File.WriteAllText(constructorTextFilePath, $"The collection open at time of instantiation was '{collectionIdentifier}'");
        }

        // reads the text file written by the constructor and displays it in a message box
        public void NonStaticMethod1()
        {
            if (constructorTextFilePath == null)
                return;

            try
            {
                MessageBox.Show(File.ReadAllText(constructorTextFilePath), "Text to file written by constructor");
            }
            catch (Exception)
            {
                return;
            }
        }

        // will increment the nonStaticMember field and display it in a message box
        // user will only see the count increment if preserve_instance is enabled, otherwise the count will always be 1 because the object is re-instantiated each time
        public void NonStaticMethod2()
        {
            nonStaticMember++;
            MessageBox.Show(nonStaticMember.ToString(), "This will increment if preserve_instance is enabled");
        }
    }
}
