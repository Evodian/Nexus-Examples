using DevExpress.XtraEditors;
using System.Windows.Forms;

namespace DevExpress_Example_Actions
{
    public class ExampleActionsWithDevExpress
    {
        public static bool DevExpressPopUp(string collectionIdentifier, string collectionAlternateIdentifier, string transactionType, int estimateRoute)
        {
            return XtraMessageBox.Show($"You have {collectionIdentifier}{(collectionAlternateIdentifier != null ? $" ({collectionAlternateIdentifier})" : "")} open.\n\nThis button was triggered from the {transactionType} tab.\n\nThe current non-stocked code estimate route open is {estimateRoute}.",
                                    "Hello, World!", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK;
        }
    }
}
