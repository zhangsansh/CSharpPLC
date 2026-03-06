namespace 西门子PLC上位机通讯软件
{
    public partial class Form2 : Form
    {
        // 存储选中的变量列表
        public List<string> SelList { get; private set; }

        // 构造函数：接收所有变量列表并初始化
        public Form2(List<string> allVars)
        {
            InitializeComponent();
            SelList = new List<string>();

            // 将所有变量加载到未选择列表（listBox1）
            listBox1.Items.AddRange(allVars.ToArray());

            // 绑定按钮点击事件
            button3.Click += btnMoveSingleRight_Click;    // >
            button4.Click += btnMoveAllRight_Click;       // >>
            button5.Click += btnMoveSingleLeft_Click;     // <
            button6.Click += btnMoveAllLeft_Click;        // <<
            button1.Click += btnConfirm_Click;            // 确认选择
            button2.Click += btnClose_Click;              // 关闭窗口
        }

        // 单个右移
        private void btnMoveSingleRight_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                listBox2.Items.Add(listBox1.SelectedItem);
                listBox1.Items.Remove(listBox1.SelectedItem);
            }
        }

        // 全部右移
        private void btnMoveAllRight_Click(object sender, EventArgs e)
        {
            listBox2.Items.AddRange(listBox1.Items);
            listBox1.Items.Clear();
        }

        // 单个左移
        private void btnMoveSingleLeft_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                listBox1.Items.Add(listBox2.SelectedItem);
                listBox2.Items.Remove(listBox2.SelectedItem);
            }
        }

        // 全部左移
        private void btnMoveAllLeft_Click(object sender, EventArgs e)
        {
            listBox1.Items.AddRange(listBox2.Items);
            listBox2.Items.Clear();
        }

        // 确认选择
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            // 收集选中的变量
            SelList = listBox2.Items.Cast<string>().ToList();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // 关闭窗口
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public Form2()
        {
            InitializeComponent();
        }
    }
}