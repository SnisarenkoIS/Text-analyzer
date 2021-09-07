using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using Exel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using Xceed.Words.NET;
using Xceed.Document.NET;
using System.Drawing.Drawing2D;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO.Compression;
using System.Security.Cryptography;

namespace Text_analyzer
{
    public partial class Form1 : Form
    {
        public static Activator activ;

        public static string[] massWord;
        public static string[] massTestDoc;
        public static string[] massWordHeader;

        public static bool Flag_click_btn_sort = false;
        public static bool Flag_click_btn_save_sort = false;
        public static bool Flag_click_btn_save_ranjir = false;

        public static Dictionary<string, int> dic_frequency = new Dictionary<string, int>(); //--- Словарь частоты встречания слов в тексте

        public static List<string> strList = new List<string>(); //--- Список со всеми считанными словами

        private static byte[] aeskey;
        private static byte[] aesiv;
        public static AesManaged AES = new AesManaged();

        public Form1()
        {
            InitializeComponent();

            dataGridView1.Visible = false;
            listBox1.Visible = false;
            dataGridView1.AllowUserToAddRows = false;
            label2.Visible = false;

            btn_sort.Enabled = false;
            btn_reset.Enabled = false;
            btn_save_sort.Enabled = false;
            btn_save_ranjir.Enabled = false;
            progressBar1.Visible = false;
            label4.Text = "";
            label5.Text = "";

            //*** Тест работы временнОй блокировки (для предотвращения неоплаты оговоренной суммы)
            string pth_DB = Environment.CurrentDirectory + "\\sys.bin";

            if (!File.Exists(pth_DB))
            {
                MessageBox.Show("Не все файлы, необходимые для запуска программы, найдены.\nОбратитесь, пожалуйста, к разработчику.");
                Activator.Flag_close_form = true;
            }
            else
            {
                byte[] bytes = File.ReadAllBytes(pth_DB);
                string txt = FromAes256(bytes);
                StringBuilder sb = new StringBuilder();

                string ch1 = Convert.ToString(txt[txt.Length - 2]);

                int num1;
                if (Int32.TryParse(ch1, out num1))
                {
                    if (num1 == 0)
                    {
                        num1 = 1;
                        for (int i = 0; i < txt.Length - 2; i++)
                        {
                            sb.Append(txt[i]);
                        }
                        sb.Append(num1.ToString());
                        sb.Append('м');

                        byte[] mass_crypt = ToAes256(Convert.ToString(sb));
                        File.WriteAllBytes(pth_DB, mass_crypt);

                        
                        Activator.Flag_close_form = false;
                    }
                    else if (num1 > 0) //--- Если прога уже один раз открывалась
                    {
                        string ch2 = Convert.ToString(txt[1]);
                        int num2;

                        if (Int32.TryParse(ch2, out num2))
                        {
                            if (num2 == 1) //--- Если прога уже один раз активировалась с помощью ключа
                            {
                                for (int i = 0; i < txt.Length; i++)
                                {
                                    sb.Append(txt[i]);
                                }

                                byte[] mass_crypt = ToAes256(Convert.ToString(sb));
                                File.WriteAllBytes(pth_DB, mass_crypt);


                            }
                            else if (num2 == 0) //--- Если прога ещё не активирована
                            {
                                activ = null;
                                activ = new Activator();
                                activ.ShowDialog();
                                string s = Activator.key;

                                if (s != key)
                                {
                                    Activator.Flag_close_form = true;

                                    for (int i = 0; i < txt.Length; i++)
                                    {
                                        sb.Append(txt[i]);
                                    }

                                    byte[] mass_crypt = ToAes256(Convert.ToString(sb));
                                    File.WriteAllBytes(pth_DB, mass_crypt);
                                }
                                else
                                {
                                    num2 = 1;
                                    sb.Append(txt[0]);
                                    sb.Append(Convert.ToString(num2));

                                    for (int i = 2; i < txt.Length; i++)
                                    {
                                        sb.Append(txt[i]);
                                    }

                                    byte[] mass_crypt = ToAes256(Convert.ToString(sb));
                                    File.WriteAllBytes(pth_DB, mass_crypt);
                                }
                            }
                        }
                    }
                }
            }
        }

        //
        //*** Метод для шифрования данных
        //
        public static byte[] ToAes256(string src)
        {
            List<byte> tmp_bytes_iv = new List<byte>();
            for (int i = 0; i < 16; i++) tmp_bytes_iv.Add((byte)IV[i]);
            aesiv = tmp_bytes_iv.ToArray();

            List<byte> tmp_bytes = new List<byte>();
            for (int i = 0; i < 16; i++) tmp_bytes.Add((byte)key[i]);
            aeskey = tmp_bytes.ToArray();

            Aes aes = Aes.Create(); //--- Объявляем объект класса AES
            aes.IV = aesiv; //--- Генерируем соль
            aes.Key = aeskey; //--- Присваиваем ключ. aeskey - переменная (массив байт), сгенерированная методом GenerateKey() класса AES

            byte[] encrypted;
            ICryptoTransform crypt = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, crypt, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(src);
                    }
                }

                encrypted = ms.ToArray(); //--- Записываем в переменную зашифрованный поток байтов
            }

            return encrypted.Concat(aes.IV).ToArray(); //--- Возвращаем поток байт + крепим соль
        }

        //
        //*** Метод для расшифрования данных
        //
        public static string FromAes256(byte[] shifr)
        {
            byte[] bytesIv = new byte[16];
            byte[] mess = new byte[shifr.Length - 16];

            for (int i = shifr.Length - 16, j = 0; i < shifr.Length; i++, j++) bytesIv[j] = shifr[i]; //--- Считываем соль
            for (int i = 0; i < shifr.Length - 16; i++) mess[i] = shifr[i]; //--- Считываем оставшуюся часть сообщения

            List<byte> tmp_bytes = new List<byte>();
            for (int i = 0; i < 16; i++) tmp_bytes.Add((byte)key[i]);
            aeskey = tmp_bytes.ToArray();

            Aes aes = Aes.Create();
            aes.Key = aeskey;
            aes.IV = bytesIv;

            string text = ""; //--- Итоговый результат
            byte[] data = mess;
            ICryptoTransform crypt = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream(data))
            {
                using (CryptoStream cs = new CryptoStream(ms, crypt, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        text = sr.ReadToEnd(); //--- Результат записываем в переменную в виде исзодной строки
                    }
                }
            }

            return text;
        }

        //
        //*** Открытие файла
        //
        private void button1_Click(object sender, EventArgs e)
        {
            if (Activator.Flag_close_form)
            {
                MessageBox.Show("Введён не правильный ключ. Приложение работать не будет до момента ввода правильного ключа.");
                this.Close();
            }

            OpenFileDialog dlg = new OpenFileDialog { Multiselect = true, Title = "Выберите файл для анализа", InitialDirectory = "C:\\" };

            dlg.ShowDialog();
            if (dlg.FileName == String.Empty) return; //--- Если ничего не выбрано - выходим

            string pth = dlg.FileName; //--- Путь до файла

            FileInfo fi = new FileInfo(pth);

            lbl_name_description.Text = fi.Name;
            lbl_size_description.Text = Convert.ToString(fi.Length);
            lbl_date_creation_description.Text = fi.CreationTime.ToShortDateString();
            lbl_time_creation_description.Text = fi.CreationTime.ToShortTimeString();
            lbl_date_final_change_description.Text = fi.LastWriteTime.ToShortDateString();

            string extension = fi.Extension;

            if (fi.Extension == ".doc")
            {
                strList.Clear();
                Word.Application wapp = new Word.Application();
                wapp.Visible = false; //--- Делаем приложение невидимым
                Object readOnly = true;

                Word.Document doc = wapp.Documents.Open(pth, ref readOnly);
                string allWords = doc.Content.Text; //--- Считали содержимое файла в строковую переменную
                doc.Close();
                wapp.Quit();

                massWord = allWords.Split(new char[] { ' ', ',', '!', '?', ':', ';', '=', '+', '*', '\\', '|', '/', '\"', '<', '>', ']', '[', '\r', '\t', '\n', '\f', '\b', '(', ')', '.', '-', '"', '"', '«', '»', '_', '•', '%', '$', '#', '&', '^', '£', '€', '¥', '{', '}', '§', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '‒', '–', '—', '―', '\0', '\a' }, StringSplitOptions.RemoveEmptyEntries); //--- получаем массив всех слов в документе

                foreach (string st in massWord)
                {
                    double tmp;
                    if (Double.TryParse(st, out tmp)) continue;
                    if (Double.TryParse(st[0].ToString(), out tmp)) continue;

                    if (st[0] == '\"' || st[0] == '«' || st[0] == '\'' || st[0] == '‘' || st[0] == '“' || st[0] == '•' || st[0] == '\r' || st[0] == 0xA0 || st[0] == '~' || st[0] == '@' || st[0] == '_' || st[0] == '”' || st[0] == '’' || st[st.Length - 1] == '\"' || st[st.Length - 1] == '»' || st[st.Length - 1] == '\'' || st[st.Length - 1] == '’' || st[st.Length - 1] == '”')
                    { 
                        StringBuilder tmp_st = new StringBuilder();

                        if (st[0] == '\"' || st[0] == '«' || st[0] == '\'' || st[0] == '‘' || st[0] == '“' || st[0] == '•' || st[0] == '\r' || st[0] == 0xA0 || st[0] == '~' || st[0] == '@' || st[0] == '_' || st[0] == '”' || st[0] == '’')
                        {
                            if (st[st.Length - 1] == '\"' || st[st.Length - 1] == '»' || st[st.Length - 1] == '\'' || st[st.Length - 1] == '’' || st[st.Length - 1] == '”') for (int i = 1; i < st.Length - 1; i++) tmp_st.Append(st[i]);
                            else                                                                                                                                            for(int i = 1; i < st.Length; i++) tmp_st.Append(st[i]);
                        }
                        else if (st[st.Length - 1] == '\"' || st[st.Length - 1] == '»' || st[st.Length - 1] == '\'' || st[st.Length - 1] == '’' || st[st.Length - 1] == '”') for (int i = 0; i < st.Length - 1; i++) tmp_st.Append(st[i]);

                        string tmp_string = Convert.ToString(tmp_st);
                        if (tmp_string.Length != 0 && (tmp_string[0] == 0xA0 || tmp_string[0] == '\a' || tmp_string[0] == 0x0020 || tmp_string[0] == ' ')) tmp_string.Remove(0, 1);
                        else if (tmp_string.Length == 0) continue;
                        else if (Double.TryParse(tmp_string[0].ToString(), out tmp)) continue;
                        else if (tmp_string.Length >= 2 && Double.TryParse(tmp_string[1].ToString(), out tmp)) continue;
                        else if (Double.TryParse(tmp_string, out tmp)) continue;

                        strList.Add(tmp_string);
                    }
                    else strList.Add(st);
                }

                lbl_total_words.Text = strList.Count.ToString();
            }
            else if (fi.Extension == ".docx")
            {
                strList.Clear();
                DocX docxLoad = DocX.Load(pth);

                var paragraphsList = docxLoad.Paragraphs;
                for (int i = 0; i < paragraphsList.Count; i++)
                {
                    massTestDoc = paragraphsList[i].Text.Split(new char[] { ' ', ',', '!', '?', ':', ';', '=', '+', '*', '\\', '|', '/', '\"', '<', '>', ']', '[', '\r', '\t', '\n', '\f', '\b', '(', ')', '.', '-', '"', '"', '«', '»', '_', '•', '%', '$', '#', '&', '^', '£', '€', '¥', '{', '}', '§', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '‒', '–', '—', '―', '\0', '\a' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string st in massTestDoc)
                    { 
                        double tmp;
                        if (Double.TryParse(st, out tmp)) continue;
                        if (Double.TryParse(st[0].ToString(), out tmp)) continue;

                        if (st[0] == '\"' || st[0] == '«' || st[0] == '\'' || st[0] == '‘' || st[0] == '“' || st[0] == '•' || st[0] == '\r' || st[0] == 0xA0 || st[0] == '~' || st[0] == '@' || st[0] == '_' || st[0] == '”' || st[0] == '’' || st[st.Length - 1] == '\"' || st[st.Length - 1] == '»' || st[st.Length - 1] == '\'' || st[st.Length - 1] == '’' || st[st.Length - 1] == '”')
                        {
                            StringBuilder tmp_st = new StringBuilder();

                            if (st[0] == '\"' || st[0] == '«' || st[0] == '\'' || st[0] == '‘' || st[0] == '“' || st[0] == '•' || st[0] == '\r' || st[0] == 0xA0 || st[0] == '~' || st[0] == '@' || st[0] == '_' || st[0] == '”' || st[0] == '’')
                            {
                                if (st[st.Length - 1] == '\"' || st[st.Length - 1] == '»' || st[st.Length - 1] == '\'' || st[st.Length - 1] == '’' || st[st.Length - 1] == '”') for (int j = 1; j < st.Length - 1; i++) tmp_st.Append(st[j]);
                                else                                                                                                                                            for (int j = 1; j < st.Length; i++) tmp_st.Append(st[j]);
                            }
                            else if (st[st.Length - 1] == '\"' || st[st.Length - 1] == '»' || st[st.Length - 1] == '\'' || st[st.Length - 1] == '’' || st[st.Length - 1] == '”') for (int j = 0; j < st.Length - 1; i++) tmp_st.Append(st[j]);

                            string tmp_string = Convert.ToString(tmp_st);
                            if (tmp_string.Length != 0 && (tmp_string[0] == 0xA0 || tmp_string[0] == '\a' || tmp_string[0] == 0x0020 || tmp_string[0] == ' ')) tmp_string.Remove(0, 1);
                            else if (tmp_string.Length == 0) continue;
                            else if (Double.TryParse(tmp_string[0].ToString(), out tmp)) continue;
                            else if (tmp_string.Length >= 2 && Double.TryParse(tmp_string[1].ToString(), out tmp)) continue;
                            else if (Double.TryParse(tmp_string, out tmp)) continue;

                            strList.Add(tmp_string);
                        }
                        else strList.Add(st);
                    }
                }

                lbl_total_words.Text = strList.Count.ToString();
            }
            else if (fi.Extension == ".txt")
            {
                strList.Clear();
                FileStream fs = new FileStream(pth, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                string s;

                while ((s = sr.ReadLine()) != null)
                {
                    massWord = s.Split(new char[] { ' ', ',', '!', '?', ':', ';', '=', '+', '*', '\\', '|', '/', '\"', '<', '>', ']', '[', '\r', '\t', '\n', '\f', '\b', '(', ')', '.', '-', '"', '"', '«', '»', '_', '•', '%', '$', '#', '&', '^', '£', '€', '¥', '{', '}', '§', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '‒', '–', '—', '―', '\0', '\a' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string st in massWord)
                    {
                        double tmp;
                        if (Double.TryParse(st, out tmp)) continue;
                        if (Double.TryParse(st[0].ToString(), out tmp)) continue;

                        if (st[0] == '\"' || st[0] == '«' || st[0] == '\'' || st[0] == '‘' || st[0] == '“' || st[0] == '•' || st[0] == '\r' || st[0] == 0xA0 || st[0] == '~' || st[0] == '@' || st[0] == '_' || st[0] == '”' || st[0] == '’' || st[st.Length - 1] == '\"' || st[st.Length - 1] == '»' || st[st.Length - 1] == '\'' || st[st.Length - 1] == '’' || st[st.Length - 1] == '”')
                        {
                            StringBuilder tmp_st = new StringBuilder();

                            if (st[0] == '\"' || st[0] == '«' || st[0] == '\'' || st[0] == '‘' || st[0] == '“' || st[0] == '•' || st[0] == '\r' || st[0] == 0xA0 || st[0] == '~' || st[0] == '@' || st[0] == '_' || st[0] == '”' || st[0] == '’')
                            {
                                if (st[st.Length - 1] == '\"' || st[st.Length - 1] == '»' || st[st.Length - 1] == '\'' || st[st.Length - 1] == '’' || st[st.Length - 1] == '”') for (int i = 1; i < st.Length - 1; i++) tmp_st.Append(st[i]);
                                else                                                                                                                                            for (int i = 1; i < st.Length; i++) tmp_st.Append(st[i]);
                            }
                            else if (st[st.Length - 1] == '\"' || st[st.Length - 1] == '»' || st[st.Length - 1] == '\'' || st[st.Length - 1] == '’' || st[st.Length - 1] == '”') for (int i = 0; i < st.Length - 1; i++) tmp_st.Append(st[i]);

                            string tmp_string = Convert.ToString(tmp_st);
                            if (tmp_string.Length != 0 && (tmp_string[0] == 0xA0 || tmp_string[0] == '\a' || tmp_string[0] == 0x0020 || tmp_string[0] == ' ')) tmp_string.Remove(0, 1);
                            else if (tmp_string.Length == 0) continue;
                            else if (Double.TryParse(tmp_string[0].ToString(), out tmp)) continue;
                            else if (tmp_string.Length >= 2 && Double.TryParse(tmp_string[1].ToString(), out tmp)) continue;
                            else if (Double.TryParse(tmp_string, out tmp)) continue;

                            strList.Add(tmp_string);
                        }
                        else strList.Add(st);
                    }
                }

                fs.Close();
                sr.Close();

                lbl_total_words.Text = strList.Count.ToString();
            }
            else if (fi.Extension == ".rtf")
            {
                strList.Clear();
                Word.Application wapp = new Word.Application();
                wapp.Visible = false; //--- Делаем приложение невидимым
                Object readOnly = true;

                Word.Document doc = wapp.Documents.Open(pth, ref readOnly);
                string allWords = doc.Content.Text;
                doc.Close();
                wapp.Quit();

                massWord = allWords.Split(new char[] { ' ', ',', '!', '?', ':', ';', '=', '+', '*', '\\', '|', '/', '\"', '<', '>', ']', '[', '\r', '\t', '\n', '\f', '\b', '(', ')', '.', '-', '"', '"', '«', '»', '_', '•', '%', '$', '#', '&', '^', '£', '€', '¥', '{', '}', '§', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '‒', '–', '—', '―' }, StringSplitOptions.RemoveEmptyEntries); //--- получаем массив всех слов в документе

                foreach (string st in massWord)
                {
                    double tmp;
                    if (Double.TryParse(st, out tmp)) continue;
                    if (st == " ") continue;
                    if (Double.TryParse(st[0].ToString(), out tmp)) continue;

                    if (st[0] == '\"' || st[0] == '«' || st[0] == '\'' || st[0] == '‘' || st[0] == '“' || st[0] == '•' || st[0] == '\r' || st[0] == 0xA0 || st[0] == '~' || st[0] == '@' || st[0] == '_' || st[0] == '”' || st[0] == '’' || st[st.Length - 1] == '\"' || st[st.Length - 1] == '»' || st[st.Length - 1] == '\'' || st[st.Length - 1] == '’' || st[st.Length - 1] == '”')
                    {
                        StringBuilder tmp_st = new StringBuilder();

                        if (st[0] == '\"' || st[0] == '«' || st[0] == '\'' || st[0] == '‘' || st[0] == '“' || st[0] == '•' || st[0] == '\r' || st[0] == 0xA0 || st[0] == '~' || st[0] == '@' || st[0] == '_' || st[0] == '”' || st[0] == '’')
                        {
                            if (st[st.Length - 1] == '\"' || st[st.Length - 1] == '»' || st[st.Length - 1] == '\'' || st[st.Length - 1] == '’' || st[st.Length - 1] == '”') for (int i = 1; i < st.Length - 1; i++) tmp_st.Append(st[i]);
                            else                                                                                                                                            for (int i = 1; i < st.Length; i++) tmp_st.Append(st[i]);
                        }
                        else if (st[st.Length - 1] == '\"' || st[st.Length - 1] == '»' || st[st.Length - 1] == '\'' || st[st.Length - 1] == '’' || st[st.Length - 1] == '”') for (int i = 0; i < st.Length - 1; i++) tmp_st.Append(st[i]);

                        string tmp_string = Convert.ToString(tmp_st);
                        if (tmp_string.Length != 0 && (tmp_string[0] == 0xA0 || tmp_string[0] == '\a' || tmp_string[0] == 0x0020 || tmp_string[0] == ' ')) tmp_string.Remove(0, 1);
                        else if (tmp_string.Length == 0) continue;
                        else if (Double.TryParse(tmp_string[0].ToString(), out tmp)) continue;
                        else if (tmp_string.Length >= 2 && Double.TryParse(tmp_string[1].ToString(), out tmp)) continue;
                        else if (Double.TryParse(tmp_string, out tmp)) continue;

                        strList.Add(tmp_string);
                    }
                    else strList.Add(st);
                }

                lbl_total_words.Text = strList.Count.ToString();

            }
            else if (fi.Extension == ".pdf")
            {
                strList.Clear();
                PdfReader reader = new PdfReader(pth);
                int intPageNum = reader.NumberOfPages;
                string[] words;
                string line;

                //--- Test progressBar
                progressBar1.Minimum = 0;
                progressBar1.Maximum = intPageNum - 1;
                progressBar1.Value = 0;
                progressBar1.Step = 1;
                progressBar1.Visible = true;
                //---
                for (int i = 1; i <= intPageNum; i++)
                {
                    label4.Text = "Идёт преобразование файла PDF.";
                    label5.Text = "Пожалуйста, подождите.";
                    progressBar1.PerformStep();

                    var text = PdfTextExtractor.GetTextFromPage(reader, i, new LocationTextExtractionStrategy());

                    words = text.Split(new char[] { ' ', ',', '!', '?', ':', ';', '=', '+', '*', '\\', '|', '/', '\"', '<', '>', ']', '[', '\r', '\t', '\n', '\f', '\b', '(', ')', '.', '-', '"', '"', '«', '»', '_', '•', '%', '$', '#', '&', '^', '£', '€', '¥', '{', '}', '§', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '‒', '–', '—', '―' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < words.Length; j++)
                    {
                        line = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(words[j]));

                        double tmp;
                        if (Double.TryParse(line, out tmp)) continue;
                        if (line == " ") continue;
                        if (Double.TryParse(line[0].ToString(), out tmp)) continue;

                        if (line[0] == '\"' || line[0] == '«' || line[0] == '\'' || line[0] == '‘' || line[0] == '“' || line[0] == '•' || line[0] == '\r' || line[0] == 0xA0 || line[0] == '~' || line[0] == '@' || line[0] == '_' || line[0] == '”' || line[0] == '’' || line[line.Length - 1] == '\"' || line[line.Length - 1] == '»' || line[line.Length - 1] == '\'' || line[line.Length - 1] == '’' || line[line.Length - 1] == '”')
                        {
                            StringBuilder tmp_st = new StringBuilder();

                            if (line[0] == '\"' || line[0] == '«' || line[0] == '\'' || line[0] == '‘' || line[0] == '“' || line[0] == '•' || line[0] == '\r' || line[0] == 0xA0 || line[0] == '~' || line[0] == '@' || line[0] == '_' || line[0] == '”' || line[0] == '’')
                            {
                                if (line[line.Length - 1] == '\"' || line[line.Length - 1] == '»' || line[line.Length - 1] == '\'' || line[line.Length - 1] == '’' || line[line.Length - 1] == '”') for (int x = 1; x < line.Length - 1; x++) tmp_st.Append(line[x]);
                                else                                                                                                                                                                for (int x = 1; x < line.Length; x++) tmp_st.Append(line[x]);
                            }
                            else if (line[line.Length - 1] == '\"' || line[line.Length - 1] == '»' || line[line.Length - 1] == '\'' || line[line.Length - 1] == '’' || line[line.Length - 1] == '”') for (int x = 0; x < line.Length - 1; x++) tmp_st.Append(line[x]);

                            string tmp_string = Convert.ToString(tmp_st);
                            if (tmp_string.Length != 0 && (tmp_string[0] == 0xA0 || tmp_string[0] == '\a' || tmp_string[0] == 0x0020 || tmp_string[0] == ' ')) tmp_string.Remove(0, 1);
                            else if (tmp_string.Length == 0) continue;
                            else if (Double.TryParse(tmp_string[0].ToString(), out tmp)) continue;
                            else if (tmp_string.Length >= 2 && Double.TryParse(tmp_string[1].ToString(), out tmp)) continue;
                            else if (Double.TryParse(tmp_string, out tmp)) continue;

                            strList.Add(tmp_string);
                        }
                        else strList.Add(line);
                    }
                }

                progressBar1.Value = 0;
                progressBar1.Visible = false;
                label4.Text = "";
                label5.Text = "";

                lbl_total_words.Text = strList.Count.ToString();
            }
            else if (fi.Extension == ".fb2")
            {
                strList.Clear();
                XmlDocument xd = new XmlDocument();

                xd.Load(pth);
                string res = Regex.Match(xd.InnerXml, @"<body>(.+?)</body>").Groups[1].Value;
                string f = Regex.Replace(res, "<[^>]+>", string.Empty);
                massWord = f.Split(new char[] { ' ', ',', '!', '?', ':', ';', '=', '+', '*', '\\', '|', '/', '\"', '<', '>', ']', '[', '\r', '\t', '\n', '\f', '\b', '(', ')', '.', '-', '"', '"', '«', '»', '_', '•', '%', '$', '#', '&', '^', '£', '€', '¥', '{', '}', '§', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '‒', '–', '—', '―', '\0', '\a' }, StringSplitOptions.RemoveEmptyEntries); //--- получаем массив всех слов в документе
                foreach (string st in massWord)
                {
                    double tmp;
                    if (Double.TryParse(st, out tmp)) continue;
                    if (Double.TryParse(st[0].ToString(), out tmp)) continue;

                    if (st[0] == '\"' || st[0] == '«' || st[0] == '\'' || st[0] == '‘' || st[0] == '“' || st[0] == '•' || st[0] == '\r' || st[0] == 0xA0 || st[0] == '~' || st[0] == '@' || st[0] == '_' || st[0] == '”' || st[0] == '’' || st[st.Length - 1] == '\"' || st[st.Length - 1] == '»' || st[st.Length - 1] == '\'' || st[st.Length - 1] == '’' || st[st.Length - 1] == '”')
                    {
                        StringBuilder tmp_st = new StringBuilder();

                        if (st[0] == '\"' || st[0] == '«' || st[0] == '\'' || st[0] == '‘' || st[0] == '“' || st[0] == '•' || st[0] == '\r' || st[0] == 0xA0 || st[0] == '~' || st[0] == '@' || st[0] == '_' || st[0] == '”' || st[0] == '’')
                        {
                            if (st[st.Length - 1] == '\"' || st[st.Length - 1] == '»' || st[st.Length - 1] == '\'' || st[st.Length - 1] == '’' || st[st.Length - 1] == '”') for (int i = 1; i < st.Length - 1; i++) tmp_st.Append(st[i]);
                            else                                                                                                                                            for (int i = 1; i < st.Length; i++) tmp_st.Append(st[i]);
                        }
                        else if (st[st.Length - 1] == '\"' || st[st.Length - 1] == '»' || st[st.Length - 1] == '\'' || st[st.Length - 1] == '’' || st[st.Length - 1] == '”') for (int i = 0; i < st.Length - 1; i++) tmp_st.Append(st[i]);

                        string tmp_string = Convert.ToString(tmp_st);
                        if (tmp_string.Length != 0 && (tmp_string[0] == 0xA0 || tmp_string[0] == '\a' || tmp_string[0] == 0x0020 || tmp_string[0] == ' ' || tmp_string[0] == 0x0000)) tmp_string.Remove(0, 1);
                        else if (tmp_string.Length == 0) continue;
                        else if (Double.TryParse(tmp_string[0].ToString(), out tmp)) continue;
                        else if (tmp_string.Length >= 2 && Double.TryParse(tmp_string[1].ToString(), out tmp)) continue;
                        else if (Double.TryParse(tmp_string, out tmp)) continue;

                        if (tmp_string[0] == 0x2026 || tmp_string[0] == '\a' || tmp_string[0] == 0x0020 || tmp_string[0] == ' ' || tmp_string[0] == 0x0000 || tmp_string[tmp_string.Length - 1] == 0x2026)
                        {
                            tmp_st = new StringBuilder();

                            if (tmp_string[tmp_string.Length - 1] == 0x2026) for (int i = 1; i < tmp_string.Length - 1; i++) tmp_st.Append(tmp_string[i]);
                            else for (int i = 1; i < tmp_string.Length; i++) tmp_st.Append(tmp_string[i]);

                            if (tmp_string.Length == 0) continue;
                            tmp_string = Convert.ToString(tmp_st);
                        }

                        strList.Add(tmp_string);
                    }
                    else strList.Add(st);
                }


                lbl_total_words.Text = strList.Count.ToString();
            }
            else if (fi.Extension == ".html")
            {
                strList.Clear();

                System.Net.WebClient wc = new System.Net.WebClient();
                string res = wc.DownloadString(pth);
                string f = Regex.Replace(res, "<[^>]+>", string.Empty);
                var fromEncoding = Encoding.Default;
                var bytes = fromEncoding.GetBytes(f);
                var toEncoding = Encoding.GetEncoding(65001);
                string d = toEncoding.GetString(bytes);

                massWord = d.Split(new char[] { ' ', ',', '!', '?', ':', ';', '=', '+', '*', '\\', '|', '/', '\"', '<', '>', ']', '[', '\r', '\t', '\n', '\f', '\b', '(', ')', '.', '-', '"', '"', '«', '»', '_', '•', '%', '$', '#', '&', '^', '£', '€', '¥', '{', '}', '§', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '‒', '–', '—', '―', '\0', '\a' }, StringSplitOptions.RemoveEmptyEntries); //--- получаем массив всех слов в документе

                foreach (string st in massWord)
                {
                    double tmp;
                    if (Double.TryParse(st, out tmp)) continue;
                    if (st == " ") continue;
                    if (Double.TryParse(st[0].ToString(), out tmp)) continue;

                    if (st[0] == '\"' || st[0] == '«' || st[0] == '\'' || st[0] == '‘' || st[0] == '“' || st[0] == '•' || st[0] == '\r' || st[0] == 0xA0 || st[0] == '~' || st[0] == '@' || st[0] == '_' || st[0] == '”' || st[0] == '’' || st[st.Length - 1] == '\"' || st[st.Length - 1] == '»' || st[st.Length - 1] == '\'' || st[st.Length - 1] == '’' || st[st.Length - 1] == '”')
                    {
                        StringBuilder tmp_st = new StringBuilder();

                        if (st[0] == '\"' || st[0] == '«' || st[0] == '\'' || st[0] == '‘' || st[0] == '“' || st[0] == '•' || st[0] == '\r' || st[0] == 0xA0 || st[0] == '~' || st[0] == '@' || st[0] == '_' || st[0] == '”' || st[0] == '’')
                        {
                            if (st[st.Length - 1] == '\"' || st[st.Length - 1] == '»' || st[st.Length - 1] == '\'' || st[st.Length - 1] == '’' || st[st.Length - 1] == '”') for (int i = 1; i < st.Length - 1; i++) tmp_st.Append(st[i]);
                            else                                                                                                                                            for (int i = 1; i < st.Length; i++) tmp_st.Append(st[i]);
                        }
                        else if (st[st.Length - 1] == '\"' || st[st.Length - 1] == '»' || st[st.Length - 1] == '\'' || st[st.Length - 1] == '’' || st[st.Length - 1] == '”') for (int i = 0; i < st.Length - 1; i++) tmp_st.Append(st[i]);

                        string tmp_string = Convert.ToString(tmp_st);
                        if (tmp_string.Length != 0 && (tmp_string[0] == 0xA0 || tmp_string[0] == '\a' || tmp_string[0] == 0x0020 || tmp_string[0] == ' ')) tmp_string.Remove(0, 1);
                        else if (tmp_string.Length == 0) continue;
                        else if (Double.TryParse(tmp_string[0].ToString(), out tmp)) continue;
                        else if (tmp_string.Length >= 2 && Double.TryParse(tmp_string[1].ToString(), out tmp)) continue;
                        else if (Double.TryParse(tmp_string, out tmp)) continue;

                        strList.Add(tmp_string);
                    }
                    else strList.Add(st);
                }

                lbl_total_words.Text = strList.Count.ToString();
            }
            else if (fi.Extension == ".odt")
            {
                FileStream fs = new FileStream(pth, FileMode.Open, FileAccess.Read);
                string res;

                var content = Unzip(fs);
                using (Stream stream = new MemoryStream(content)) res = ClearXML(stream);

                string[] tmp_massWord = res.Split(new char[] { ' ', ',', '!', '?', ':', ';', '=', '+', '*', '\\', '|', '/', '\"', '<', '>', ']', '[', '\r', '\t', '\n', '\f', '\b', '(', ')', '.', '-', '"', '"', '«', '»', '_', '•', '%', '$', '#', '&', '^', '£', '€', '¥', '{', '}', '§', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '‒', '–', '—', '―', '\0', '\a' }, StringSplitOptions.RemoveEmptyEntries); //--- получаем массив всех слов в документе

                List<string> mass_tmp = new List<string>();

                foreach (string s in tmp_massWord)
                {
                    if (tags.Contains(s)) continue;
                    mass_tmp.Add(s);
                }

                massWord = mass_tmp.ToArray();

                foreach (string st in massWord)
                {
                    double tmp;
                    if (Double.TryParse(st, out tmp)) continue;
                    if (st == " ") continue;
                    if (Double.TryParse(st[0].ToString(), out tmp)) continue;

                    if (st[0] == '\"' || st[0] == '«' || st[0] == '\'' || st[0] == '‘' || st[0] == '“' || st[0] == '•' || st[0] == '\r' || st[0] == 0xA0 || st[0] == '~' || st[0] == '@' || st[0] == '_' || st[0] == '”' || st[0] == '’' || st[st.Length - 1] == '\"' || st[st.Length - 1] == '»' || st[st.Length - 1] == '\'' || st[st.Length - 1] == '’' || st[st.Length - 1] == '”')
                    {
                        StringBuilder tmp_st = new StringBuilder();

                        if (st[0] == '\"' || st[0] == '«' || st[0] == '\'' || st[0] == '‘' || st[0] == '“' || st[0] == '•' || st[0] == '\r' || st[0] == 0xA0 || st[0] == '~' || st[0] == '@' || st[0] == '_' || st[0] == '”' || st[0] == '’')
                        {
                            if (st[st.Length - 1] == '\"' || st[st.Length - 1] == '»' || st[st.Length - 1] == '\'' || st[st.Length - 1] == '’' || st[st.Length - 1] == '”') for (int i = 1; i < st.Length - 1; i++) tmp_st.Append(st[i]);
                            else                                                                                                                                            for (int i = 1; i < st.Length; i++) tmp_st.Append(st[i]);
                        }
                        else if (st[st.Length - 1] == '\"' || st[st.Length - 1] == '»' || st[st.Length - 1] == '\'' || st[st.Length - 1] == '’' || st[st.Length - 1] == '”') for (int i = 0; i < st.Length - 1; i++) tmp_st.Append(st[i]);

                        string tmp_string = Convert.ToString(tmp_st);
                        if (tmp_string.Length != 0 && (tmp_string[0] == 0xA0 || tmp_string[0] == '\a' || tmp_string[0] == 0x0020 || tmp_string[0] == ' ')) tmp_string.Remove(0, 1);
                        else if (tmp_string.Length == 0) continue;
                        else if (Double.TryParse(tmp_string[0].ToString(), out tmp)) continue;
                        else if (tmp_string.Length >= 2 && Double.TryParse(tmp_string[1].ToString(), out tmp)) continue;
                        else if (Double.TryParse(tmp_string, out tmp)) continue;

                        if (tmp_string[0] == 0x2026 || tmp_string[tmp_string.Length - 1] == 0x2026)
                        {
                            tmp_st = new StringBuilder();

                            if (tmp_string[tmp_string.Length - 1] == 0x2026) for (int i = 1; i < tmp_string.Length - 1; i++) tmp_st.Append(tmp_string[i]);
                            else                                             for (int i = 1; i < tmp_string.Length; i++) tmp_st.Append(tmp_string[i]);

                            if (tmp_string.Length == 0) continue;
                            tmp_string = Convert.ToString(tmp_st);
                        }

                        strList.Add(tmp_string);
                    }
                    else strList.Add(st);
                }

                lbl_total_words.Text = strList.Count.ToString();
            }

            btn_sort.Enabled = true;
            btn_reset.Enabled = true;
            btn_save_sort.Enabled = false;
            btn_save_ranjir.Enabled = false;
            btn_open_file.Enabled = false;
        }

        //
        //*** Функция, открывающая поток как архив 
        //
        private byte[] Unzip(Stream stream) 
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ZipArchive archive = new ZipArchive(stream);
                var unzippedEntryStream = archive.GetEntry("content.xml").Open();
                unzippedEntryStream.CopyTo(ms);
                return ms.ToArray();
            }
        }

        //
        //***Функция обработки исходного файла .xml
        //
        private static string IV = "~?????0?\b?D{><";
        private string ClearXML(Stream xmlStream)
        {
            XmlWriterSettings settings = new XmlWriterSettings(); //--- Создаём настройки XmlWriter
            settings.ConformanceLevel = ConformanceLevel.Auto; //--- Необходимый параметр для формирования вложенности тегов
            StringBuilder sb = new StringBuilder(); //--- XmlWriter будем вести запись в StringBuilder;

            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                XmlReader reader = XmlReader.Create(xmlStream);
                reader.ReadToFollowing("office:body");
                while (reader.Read())
                {
                    MethodSwitcher(reader, writer);
                }
            }
            return sb.ToString();
        }

        private void MethodSwitcher(XmlReader reader, XmlWriter writer)
        {
            switch (reader.NodeType)
            { 
                case XmlNodeType.Element:
                    if (!reader.IsEmptyElement || reader.Name == "text:s")
                    {
                        TagWriter(reader, writer);
                    }
                    break;
                case XmlNodeType.EndElement:
                    if (tags.Contains(reader.LocalName))
                    {
                        writer.WriteEndElement();
                        writer.Flush();
                    }
                    break;
                case XmlNodeType.Text:
                    writer.WriteString(reader.Value);
                    break;
                default: break;
            }
        }

        private readonly List<string> tags = new List<string>() { "p", "table", "table-row", "table-cell", "list", "list-item" }; //--- Коллекция допустимых тегов

        private void TagWriter(XmlReader reader, XmlWriter writer)
        {
            switch (reader.LocalName)
            { 
                case "p":
                    writer.WriteStartElement("p");
                    break;
                case "table":
                    writer.WriteStartElement("table");
                    break;
                case "table-row":
                    writer.WriteStartElement("row");
                    break;
                case "table-cell":
                    writer.WriteStartElement("cell");
                    break;
                case "list":
                    writer.WriteStartElement("list");
                    break;
                case "list-item":
                    writer.WriteStartElement("item");
                    break;
                case "s":
                    writer.WriteString(" ");
                    break;
                default: break;
            }
        }


        //
        //*** Кнопка, запускающая сортировку
        //
        DataTable dt;
        private void btn_sort_Click(object sender, EventArgs e)
        {
            Flag_click_btn_sort = true;
            dic_frequency.Clear();

            strList.Sort(); //--- Сортируем массив

            //--- Высчитываем частоту встречания слов в тексте
            foreach (string strin in strList)
            {
                if (dic_frequency.ContainsKey(strin)) //--- Если слово из файла есть в словаре
                {
                    int tmp_exe = dic_frequency[strin]; //--- Получаем значение по ключу
                    tmp_exe++; //--- Увеличиваем число вхождений на 1
                    dic_frequency[strin] = tmp_exe;
                }
                else dic_frequency[strin] = 1;
            }

            dic_frequency = dic_frequency.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value); //--- Сортируем словарь по значениям (по частоте)

            label3.Text = dic_frequency.Count.ToString();
            dataGridView1.Visible = true;
            listBox1.Visible = true;

            foreach (string s in strList) if (!listBox1.Items.Contains(s)) listBox1.Items.Add(s);

            dt = new DataTable();
            dt.Columns.Add("Слово");
            dt.Columns.Add("Частота");

            foreach (var item in dic_frequency)
            {
                DataRow r = dt.NewRow();
                r["Слово"] = item.Key;
                r["Частота"] = item.Value;
                dt.Rows.Add(r);
            }

            dataGridView1.DataSource = dt;
            label2.Visible = true;

            btn_sort.Enabled = false;
            btn_reset.Enabled = true;
            btn_save_sort.Enabled = true;
            btn_save_ranjir.Enabled = true;
            btn_open_file.Enabled = false;
        }

        //
        //*** Добавление градиента
        //
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            LinearGradientBrush lgb = new LinearGradientBrush(new Point(0, 0), new Point(529, 546), Color.Aqua, Color.Azure);

            g.FillRectangle(lgb, this.ClientRectangle);
            lgb.Dispose();
        }

        //
        //*** Обработка кнопки полного сброса
        //
        private void btn_reset_Click(object sender, EventArgs e)
        {
            lbl_name_description.Text = "";
            lbl_size_description.Text = "";
            lbl_date_creation_description.Text = "";
            lbl_time_creation_description.Text = "";
            lbl_date_final_change_description.Text = "";
            lbl_total_words.Text = "";
            label3.Text = "";

            dt.Clear();
            listBox1.Items.Clear();

            dataGridView1.Visible = false;
            listBox1.Visible = false;
            label2.Visible = false;

            btn_sort.Enabled = false;
            btn_reset.Enabled = false;
            btn_save_sort.Enabled = false;
            btn_save_ranjir.Enabled = false;
            btn_open_file.Enabled = true;

            Flag_click_btn_sort = false;
            Flag_click_btn_save_sort = false;
            Flag_click_btn_save_ranjir = false;
        }

        private void btn_save_sort_Click(object sender, EventArgs e)
        {
            Flag_click_btn_save_sort = true;

            string pathDir = Directory.GetCurrentDirectory() + "\\Отчёты о частоте использования слов в тексте."; //---- Получаем путь до папки, из которой запускаемся
            if (!Directory.Exists(pathDir))//----- Если искомой папки нет - создаем
            {
                Directory.CreateDirectory(pathDir);
            }

            DirectoryInfo di_tmp = new DirectoryInfo(pathDir);
            FileInfo[] fi_tmp = di_tmp.GetFiles();

            DateTime dt = DateTime.Now;
            string date = dt.Day.ToString("d2") + "." + dt.Month.ToString("d2") + "." + (dt.Year).ToString("d2");
            string time = dt.Hour.ToString("d2") + ":" + dt.Minute.ToString("d2");
            string pthDocument = pathDir + "\\Отчёт №" + (fi_tmp.Length + 1) + " от " + date + "г.docx"; //--- Полный путь до файла

            object fname = pthDocument;

            Word.Application wordapp = new Word.Application();
            wordapp.Visible = false;

            object missing = Type.Missing;
            Word.Document worddoc = wordapp.Documents.Add(ref missing, ref missing, ref missing, ref missing); //--- Создаём новый документ

            Word.Paragraph para;

            para = worddoc.Paragraphs.Add(ref missing); //--- Добавляем новый параграф

            para.Range.Text = "Отчёт от " + date + ".";
            object style_name = "Заголовок 1";
            para.Range.set_Style(ref style_name); //--- Задаём стиль
            para.Range.Font.Name = "Times New Roman";
            para.Range.Font.Position = 20; //--- Задаём расстояние между заголовком и след. строкой
            para.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter; //--- Центруем
            para.Range.Font.Color = Word.WdColor.wdColorBlack; //--- Выставляем чёрный цвет
            para.Range.Font.Size = 20; //--- Выставили размер шрифта
            para.Range.Font.Bold = 1; //--- Показали, что строка будет жирной (очень жирной)
            para.Range.InsertParagraphAfter(); //--- Добавляем параграф в документ

            para.Range.Font.Name = "Times New Roman";
            para.Range.Text = "\n«Частота встречаемости слов в тексте»";
            para.Range.Font.Position = 0; //--- Задаём расстояние между заголовком и след. строкой
            para.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            para.Range.Font.Color = Word.WdColor.wdColorBlack; //--- Выставляем чёрный цвет
            para.Range.Font.Size = 14; //--- Выставили размер шрифта
            para.Range.Font.Bold = 1; //--- Показали, что строка будет жирной (очень жирной)
            para.Range.InsertParagraphAfter(); //--- Добавляем параграф в документ

            para.Range.Font.Name = "Times New Roman";
            para.Range.Text = "\n";
            para.Range.Font.Position = 0; //--- Задаём расстояние между заголовком и след. строкой
            para.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            para.Range.Font.Color = Word.WdColor.wdColorBlack; //--- Выставляем чёрный цвет
            para.Range.Font.Size = 14; //--- Выставили размер шрифта
            para.Range.Font.Bold = 0; //--- Показали, что строка будет жирной (очень жирной)
            para.Range.InsertParagraphAfter(); //--- Добавляем параграф в документ

            para.Range.Font.Name = "Times New Roman";
            para.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            para.Range.Text = "\tСегодня, " + date + ", был произведён анализ файла (" + lbl_name_description.Text + ") на выявление частоты встречаемости слов в тексте.\n\tВ ходе анализа были выявлены следующие частоты встречаемости.";
            para.Range.Font.Position = 0; //--- Задаём расстояние между заголовком и след. строкой
            para.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            para.Range.Font.Color = Word.WdColor.wdColorBlack; //--- Выставляем чёрный цвет
            para.Range.Font.Size = 14; //--- Выставили размер шрифта
            para.Range.Font.Bold = 0; //--- Показали, что строка будет жирной (очень жирной)
            para.Range.InsertParagraphAfter(); //--- Добавляем параграф в документ

            para = worddoc.Paragraphs.Add();
            worddoc.Tables.Add(para.Range, dataGridView1.Rows.Count + 1, 3);
            var table = worddoc.Tables[worddoc.Tables.Count];
            table.set_Style("Сетка таблицы");
            table.ApplyStyleHeadingRows = true;
            table.ApplyStyleLastRow = false;
            table.ApplyStyleFirstColumn = true;
            table.ApplyStyleLastColumn = false;
            table.ApplyStyleRowBands = true;
            table.ApplyStyleColumnBands = false;

            table.Cell(1, 1).Range.InsertAfter("№");
            table.Cell(1, 2).Range.InsertAfter("Слово");
            table.Cell(1, 3).Range.InsertAfter("Частота");

            //--- Test progressBar
            progressBar1.Minimum = 0;
            progressBar1.Maximum = dataGridView1.Rows.Count - 1;
            progressBar1.Value = 0;
            progressBar1.Step = 1;
            progressBar1.Visible = true;
            label4.Text = "Идёт сохранение данных в файл (по частоте).";
            label5.Text = "Пожалуйста, подождите.";
            //---
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                progressBar1.PerformStep();

                table.Cell(i + 2, 1).Range.InsertAfter(Convert.ToString(i + 1));
                table.Cell(i + 2, 2).Range.InsertAfter(Convert.ToString(dataGridView1.Rows[i].Cells[0].Value));
                table.Cell(i + 2, 3).Range.InsertAfter(Convert.ToString(dataGridView1.Rows[i].Cells[1].Value));
            }

            worddoc.SaveAs(ref fname, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);

            object save_changes = false;
            worddoc.Close(ref save_changes, ref missing, ref missing);
            wordapp.Quit(ref save_changes, ref missing, ref missing);

            if (File.Exists(pthDocument))
            {
                MessageBox.Show("Файл создан.\nПуть до файла: " + pathDir);
                progressBar1.Value = 0;
                progressBar1.Visible = false;
                label4.Text = "";
                label5.Text = "";
            }
            else MessageBox.Show("Не удалось создать файл.");
        }
        private static string key = "јqMЧACe»N(кЉщПУQ°ФF\\\\ў¬ф%h™C|";
        private void btn_save_ranjir_Click(object sender, EventArgs e)
        {
            Flag_click_btn_save_ranjir = true;

            string pathDir = Directory.GetCurrentDirectory() + "\\Список слов из анализируемого текста, отсортированный по алфавиту."; //---- Получаем путь до папки, из которой запускаемся
            if (!Directory.Exists(pathDir))//----- Если искомой папки нет - создаем
            {
                Directory.CreateDirectory(pathDir);
            }

            DirectoryInfo di_tmp = new DirectoryInfo(pathDir);
            FileInfo[] fi_tmp = di_tmp.GetFiles();

            DateTime dt = DateTime.Now;
            string date = dt.Day.ToString("d2") + "." + dt.Month.ToString("d2") + "." + (dt.Year).ToString("d2");
            string time = dt.Hour.ToString("d2") + ":" + dt.Minute.ToString("d2");
            string pthDocument = pathDir + "\\Отчёт №" + (fi_tmp.Length + 1) + " от " + date + "г.docx"; //--- Полный путь до файла

            object fname = pthDocument;

            Word.Application wordapp = new Word.Application();
            wordapp.Visible = false;

            object missing = Type.Missing;
            Word.Document worddoc = wordapp.Documents.Add(ref missing, ref missing, ref missing, ref missing); //--- Создаём новый документ

            Word.Paragraph para;

            para = worddoc.Paragraphs.Add(ref missing); //--- Добавляем новый параграф

            para.Range.Text = "Отчёт от " + date + ".";
            object style_name = "Заголовок 1";
            para.Range.set_Style(ref style_name); //--- Задаём стиль
            para.Range.Font.Name = "Times New Roman";
            para.Range.Font.Position = 20; //--- Задаём расстояние между заголовком и след. строкой
            para.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter; //--- Центруем
            para.Range.Font.Color = Word.WdColor.wdColorBlack; //--- Выставляем чёрный цвет
            para.Range.Font.Size = 20; //--- Выставили размер шрифта
            para.Range.Font.Bold = 1; //--- Показали, что строка будет жирной (очень жирной)
            para.Range.InsertParagraphAfter(); //--- Добавляем параграф в документ

            para.Range.Font.Name = "Times New Roman";
            para.Range.Text = "\n«Слова из анализируемого текста, отсортированные по алфавиту.»";
            para.Range.Font.Position = 0; //--- Задаём расстояние между заголовком и след. строкой
            para.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            para.Range.Font.Color = Word.WdColor.wdColorBlack; //--- Выставляем чёрный цвет
            para.Range.Font.Size = 14; //--- Выставили размер шрифта
            para.Range.Font.Bold = 1; //--- Показали, что строка будет жирной (очень жирной)
            para.Range.InsertParagraphAfter(); //--- Добавляем параграф в документ

            para.Range.Font.Name = "Times New Roman";
            para.Range.Text = "\n";
            para.Range.Font.Position = 0; //--- Задаём расстояние между заголовком и след. строкой
            para.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            para.Range.Font.Color = Word.WdColor.wdColorBlack; //--- Выставляем чёрный цвет
            para.Range.Font.Size = 14; //--- Выставили размер шрифта
            para.Range.Font.Bold = 0; //--- Показали, что строка будет жирной (очень жирной)
            para.Range.InsertParagraphAfter(); //--- Добавляем параграф в документ

            para = worddoc.Paragraphs.Add();
            worddoc.Tables.Add(para.Range, listBox1.Items.Count + 1, 2);
            var table = worddoc.Tables[worddoc.Tables.Count];
            table.set_Style("Сетка таблицы");
            table.ApplyStyleHeadingRows = true;
            table.ApplyStyleLastRow = false;
            table.ApplyStyleFirstColumn = true;
            table.ApplyStyleLastColumn = false;
            table.ApplyStyleRowBands = true;
            table.ApplyStyleColumnBands = false;

            table.Cell(1, 1).Range.InsertAfter("№");
            table.Cell(1, 2).Range.InsertAfter("Слово");

            //--- Test progressBar
            progressBar1.Minimum = 0;
            progressBar1.Maximum = dataGridView1.Rows.Count - 1;
            progressBar1.Value = 0;
            progressBar1.Step = 1;
            progressBar1.Visible = true;
            label4.Text = "Идёт сохранение данных в файл (по алфавиту).";
            label5.Text = "Пожалуйста, подождите.";
            //---
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                progressBar1.PerformStep();

                table.Cell(i + 2, 1).Range.InsertAfter(Convert.ToString(i + 1));
                table.Cell(i + 2, 2).Range.InsertAfter(listBox1.Items[i].ToString());
            }

            worddoc.SaveAs(ref fname, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);

            object save_changes = false;
            worddoc.Close(ref save_changes, ref missing, ref missing);
            wordapp.Quit(ref save_changes, ref missing, ref missing);

            if (File.Exists(pthDocument))
            {
                MessageBox.Show("Файл создан.\nПуть до файла: " + pathDir);
                progressBar1.Value = 0;
                progressBar1.Visible = false;
                label4.Text = "";
                label5.Text = "";
            }
            else MessageBox.Show("Не удалось создать файл.");
        }
    }
}
