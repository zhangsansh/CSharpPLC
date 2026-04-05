using S7CommPlusDriver;
using System.Text;

namespace 西门子PLC上位机通讯软件
{
    public partial class Form1 : Form
    {
        private S7CommPlusConnection conn = new S7CommPlusConnection();
        private bool isConnected = false;
        private CancellationTokenSource cts;
        private bool isReading = false;
        private List<VarInfo> AllVarInfo = new List<VarInfo>();

        // 补充缺失的锁对象（线程安全）
        private readonly object objlock = new object();

        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
                this.btn_Connect.Text = isConnected ? "断开连接" : "建立连接";
            }
        }

        public Form1()
        {
            InitializeComponent();
            cts = new CancellationTokenSource();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataGridViewTextBoxColumn textBoxColumn = new DataGridViewTextBoxColumn();
            textBoxColumn.Name = "Column1";
            textBoxColumn.HeaderText = "变量要求";
            textBoxColumn.Width = 220;
            textBoxColumn.ReadOnly = false;
            textBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            DataGridViewTextBoxColumn textBoxColumn1 = new DataGridViewTextBoxColumn();
            textBoxColumn1.Name = "Column2";
            textBoxColumn1.HeaderText = "访问地址";
            textBoxColumn1.Width = 150;
            textBoxColumn1.ReadOnly = false;
            textBoxColumn1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            DataGridViewTextBoxColumn textBoxColumn2 = new DataGridViewTextBoxColumn();
            textBoxColumn2.Name = "Column3";
            textBoxColumn2.HeaderText = "数据类型";
            textBoxColumn2.Width = 150;
            textBoxColumn2.ReadOnly = false;
            textBoxColumn2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            DataGridViewTextBoxColumn textBoxColumn3 = new DataGridViewTextBoxColumn();
            textBoxColumn3.Name = "Column4";
            textBoxColumn3.HeaderText = "实时值";
            textBoxColumn3.Width = 150;
            textBoxColumn3.ReadOnly = true;  // 实时值设为只读，避免手动修改
            textBoxColumn3.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dataGridView1.Columns.Add(textBoxColumn);
            dataGridView1.Columns.Add(textBoxColumn1);
            dataGridView1.Columns.Add(textBoxColumn2);
            dataGridView1.Columns.Add(textBoxColumn3);
        }

        // 1. 改进的连接方法 - 添加更完善的错误处理
        private void btn_Connect_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                // IP校验
                if (string.IsNullOrWhiteSpace(txt_IP.Text) ||
                    !System.Net.IPAddress.TryParse(txt_IP.Text, out _))
                {
                    MessageBox.Show("请输入有效的IP地址", "错误");
                    return;
                }

                // 关键：S7CommPlus通常使用端口102（标准S7端口）
                // 确保conn.Connect方法内部使用了正确的端口和协议版本
                int res = conn.Connect(txt_IP.Text.Trim());

                if (res == 0)
                {
                    IsConnected = true;
                    MessageBox.Show("PLC连接成功", "提示");
                }
                else
                {
                    // 添加详细的错误码解析
                    string errMsg = GetErrorMessage(res);
                    MessageBox.Show($"PLC连接失败\n错误码: {res}\n{errMsg}", "连接错误");
                }
            }
            else
            {
                DisconnectPLC();
            }
        }

        // 2. 添加错误码解析辅助方法
        private string GetErrorMessage(int errorCode)
        {
            return errorCode switch
            {
                0 => "成功",
                1 => "连接被拒绝 - 检查IP和端口",
                2 => "PDU格式错误 - 检查PLC配置(Put/Get权限)",
                3 => "访问被拒绝 - 检查DB块是否取消优化访问",
                4 => "地址无效 - 检查DB块是否存在",
                5 => "数据类型不匹配",
                _ => $"未知错误 (代码: {errorCode})"
            };
        }

        // 3. 改进的断开连接方法
        private void DisconnectPLC()
        {
            try
            {
                cts?.Cancel();
                isReading = false;

                // 等待读取任务完全停止
                Task.Delay(200).Wait();

                conn?.Disconnect();
                IsConnected = false;
                btn_Read.Text = "开始读取";
                MessageBox.Show("已断开PLC连接", "提示");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"断开连接失败：{ex.Message}", "错误");
            }
        }

        private void btn_VarConfig_Click(object sender, EventArgs e)
        {
            if (IsConnected == false)
            {
                MessageBox.Show("请先连接PLC", "变量配置");
                return;
            }

            int res = conn.Browse(out AllVarInfo);
            if (res == 0 && AllVarInfo.Count > 0)
            {
                Form2 form2 = new Form2(AllVarInfo.Select(c => c.Name).ToList());
                DialogResult dialogResult = form2.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    this.dataGridView1.Rows.Clear();
                    // 修复语法错误：移除多余的=
                    List<string> list = form2.SelList;
                    foreach (var item in list)
                    {
                        VarInfo? varInfo = AllVarInfo.Find(c => c.Name == item);
                        if (varInfo != null)
                        {
                            int index = this.dataGridView1.Rows.Add();
                            this.dataGridView1.Rows[index].Cells[0].Value = varInfo.Name;
                            this.dataGridView1.Rows[index].Cells[1].Value = varInfo.AccessSequence;
                            this.dataGridView1.Rows[index].Cells[2].Value = Softdatatype.Types.ContainsKey(varInfo.Softdatatype)
                                ? Softdatatype.Types[varInfo.Softdatatype] : "未知类型";
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("获取标签列表失败或无变量", "变量配置");
            }
        }

        private void btn_Read_Click(object sender, EventArgs e)
        {
            if (IsConnected == false)
            {
                MessageBox.Show("请先连接PLC", "变量读取");
                return;
            }

            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("请先配置要读取的变量", "变量读取");
                return;
            }

            if (this.btn_Read.Text == "开始读取")
            {
                List<ItemAddress> readlist = new List<ItemAddress>();
                foreach (DataGridViewRow item in this.dataGridView1.Rows)
                {
                    if (item.Cells[1].Value != null && !string.IsNullOrWhiteSpace(item.Cells[1].Value.ToString()))
                    {
                        readlist.Add(new ItemAddress(item.Cells[1].Value.ToString()));
                    }
                }

                if (readlist.Count == 0)
                {
                    MessageBox.Show("无有效变量地址可读取", "变量读取");
                    return;
                }

                cts = new CancellationTokenSource();
                isReading = true;

                // 异步读取数据（修复线程安全问题：使用Invoke更新UI）
                Task.Run(async () =>
                {
                    while (!cts.IsCancellationRequested && isReading)
                    {
                        try
                        {
                            lock (this.objlock)
                            {
                                List<object> values = new List<object>();
                                List<ulong> errors = new List<ulong>();
                                int res = conn.ReadValues(readlist, out values, out errors);

                                // 更新UI（必须通过Invoke）
                                this.Invoke(new Action(() =>
                                {
                                    if (res == 0 && values.Count == readlist.Count)
                                    {
                                        for (int i = 0; i < readlist.Count; i++)
                                        {
                                            if (i < dataGridView1.Rows.Count)
                                            {
                                                dataGridView1.Rows[i].Cells[3].Value = GetActualValue(values[i]);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine($"读取失败：错误码={res}，值数量={values.Count}，地址数量={readlist.Count}");
                                    }
                                }));
                            }
                            // 读取间隔（避免高频读取）
                            await Task.Delay(500, cts.Token);
                        }
                        catch (TaskCanceledException)
                        {
                            // 取消任务时正常退出
                            break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"读取异常：{ex.Message}");
                            await Task.Delay(1000);
                        }
                    }
                }, cts.Token);

                this.btn_Read.Text = "停止读取";
            }
            else
            {
                cts?.Cancel();
                isReading = false;
                this.btn_Read.Text = "开始读取";
            }
        }

        // 修复GetValue方法的语法错误
        private object GetActualValue(object value)
        {
            if (value == null) return string.Empty;

            if (value is ValueBool)
            {
                return ((ValueBool)value).GetValue();
            }
            else if (value is ValueUSInt)
            {
                return ((ValueUSInt)value).GetValue();
            }
            else if (value is ValueUSIntArray)
            {
                // 修复语法错误：补全代码
                byte[] bytes = ((ValueUSIntArray)value).GetValue();
                return Encoding.GetEncoding("GBK").GetString(bytes);
            }
            else if (value is ValueByte)
            {
                return ((ValueByte)value).GetValue();
            }
            else if (value is ValueUInt)
            {
                return ((ValueUInt)value).GetValue();
            }
            else if (value is ValueUIntArray)
            {
                return ((ValueUIntArray)value).GetValue();
            }
            else if (value is ValueUDInt)
            {
                return ((ValueUDInt)value).GetValue();
            }
            else if (value is ValueULInt)
            {
                return ((ValueULInt)value).GetValue();
            }
            else if (value is ValueSInt)
            {
                return ((ValueSInt)value).GetValue();
            }
            else if (value is ValueInt)
            {
                return ((ValueInt)value).GetValue();
            }
            else if (value is ValueDInt)
            {
                return ((ValueDInt)value).GetValue();
            }
            else if (value is ValueLInt)
            {
                return ((ValueLInt)value).GetValue();
            }
            else if (value is ValueWord)
            {
                return ((ValueWord)value).GetValue();
            }
            else if (value is ValueDWord)
            {
                return ((ValueDWord)value).GetValue();
            }
            else if (value is ValueLWord)
            {
                return ((ValueLWord)value).GetValue();
            }
            else if (value is ValueReal)
            {
                return ((ValueReal)value).GetValue();
            }
            else if (value is ValueLReal)
            {
                return ((ValueLReal)value).GetValue();
            }
            else if (value is ValueTimestamp)
            {
                return ((ValueTimestamp)value).GetValue();
            }
            else if (value is ValueTimespan)
            {
                return ((ValueTimespan)value).GetValue();
            }
            else if (value is ValueRID)
            {
                return ((ValueRID)value).GetValue();
            }
            else if (value is ValueAID)
            {
                return ((ValueAID)value).GetValue();
            }
            else if (value is ValueBlob)
            {
                return ((ValueBlob)value).GetValue();
            }
            else if (value is ValueWString)
            {
                return ((ValueWString)value).GetValue();
            }
            else if (value is ValueStruct)
            {
                return ((ValueStruct)value).GetValue();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}