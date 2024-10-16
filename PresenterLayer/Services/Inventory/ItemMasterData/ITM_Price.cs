using System.Windows.Forms;

namespace PresenterLayer.Services
{
    class ITM_Price
    {
        public void Price_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.'))
            { e.Handled = true; }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            { e.Handled = true; }
        }
    }
}
