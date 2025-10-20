using TClassificacaoTributaria;

namespace DesktopApp;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void button1_Click_1(object sender, EventArgs e)
    {
        var classificacaoTributaria = ClassificacaoTributaria.FromJson(richText.Text);
    }
}