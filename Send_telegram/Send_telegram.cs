using DesignUI_Send_telegram.Components;
using DesignUI_Send_telegram.Controls;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using TeleSharp.TL;
using TLSharp.Core;
using Microsoft.Win32;
using System.Collections.Generic;

namespace DesignUI_Send_telegram
{
    // Класс Send_telegram основной формы приложения Send_telegram 

    public partial class Send_telegram : ShadowedForm
    {
        //Значение переменных apiId и apiHash взяты с  https://my.telegram.org/apps 

        private readonly static int apiId = 8164488;
        private readonly static string apiHash = "ee92079c288690b01e5c22d018789808";

        static string phoneNumber = "";

        TelegramClient client = new TelegramClient(apiId, apiHash);
        //Конструктор формы Send_telegram    

        //Переменная ID - идентификатор контакта в списке всех контактов по подключенной сессии

        int ID = 0;
        const int WinDefaultDPI = 96;

        /// <summary>
        /// Исправление блюра при включенном масштабировании в ОС windows 8 и выше
        /// </summary>
        public static void DpiFix()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDPIAware();
            }
        }

        /// <summary>
        /// WinAPI SetProcessDPIAware 
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        /// <summary>
        /// Исправление размера шрифтов
        /// </summary>
        /// <param name="c"></param>
        public static float DpiFixFonts(Control c)
        {
            Graphics g = c.CreateGraphics();
            float dx = g.DpiX
                , dy = g.DpiY
                , fontsScale = Math.Max(dx, dy) / WinDefaultDPI
            ;
            g.Dispose();
            return fontsScale;
        }

        public Send_telegram()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            Animator.Start();
            this.Font = SystemFonts.IconTitleFont;
            SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            //DpiFix();
            //yt_Button yt_Button = new yt_Button();
            //OthersMethods.GetAllControls(yt_Button11, yt_Button.GetType);
            yt_Button[] MassAll = new yt_Button[27]
            {                  
         yt_Button11,   
         yt_Button16,
         yt_Button14,   
         yt_Button48,    
         yt_Button50,
         yt_Button52,
         yt_Button54,
         yt_Button34,
         yt_Button40,  
         yt_Button42,   
         yt_Button44, 
         yt_Button46,   
         yt_Button20,
         yt_Button24,      
         yt_Button4,        
         yt_Button18,     
         yt_Button26,     
         yt_Button32,
         yt_Button6,
         yt_Button22,   
         yt_Button36,     
         yt_Button74,    
         yt_Button72,     
         yt_Button70,    
         yt_Button68,      
         yt_Button66,  
         yt_Button64,     
    };
            yt_Button[] MassGreen = new yt_Button[27]
          {
              yt_Button75,
               yt_Button71,
                 yt_Button73,
                   yt_Button69,
                     yt_Button67,
                       yt_Button33,
                           yt_Button65,
                              yt_Button63,
                                   yt_Button53,
                                         yt_Button45,
                                               yt_Button43,
                                                     yt_Button51,
                                                              yt_Button12,
                                                                     yt_Button13,
                                                                        yt_Button19,
                                                                             yt_Button47,
                                                                                 yt_Button49, 
              yt_Button3,
                      yt_Button5,
                            yt_Button41,
                                    yt_Button39,
                                     yt_Button17,
                                          yt_Button15,
                                                 yt_Button21,
                                                     yt_Button31,
                                                        yt_Button25,
                                                          yt_Button35,

          };
            yt_Button[] MassText = new yt_Button[6]
        {
                  yt_Button55,
                           yt_Button59,
                                 yt_Button58,
                                   yt_Button57,
                                        yt_Button56,
                                           yt_Button60,
        };
            yt_Button[] Mass_Button = new yt_Button[MassAll.Length + MassGreen.Length + MassText.Length];
            for (int i = 0; i < Mass_Button.Length; i++)
            {
                if (i > -1 & i < MassAll.Length)
                    Mass_Button[i] = MassAll[i];
                if (i > MassAll.Length - 1 & i < MassAll.Length + MassGreen.Length)
                    Mass_Button[i] = MassGreen[i - MassAll.Length];
                if (i > (MassAll.Length + MassGreen.Length) - 1 & i < MassAll.Length + MassGreen.Length + MassText.Length)
                    Mass_Button[i] = MassText[i - (MassAll.Length + MassGreen.Length)];
            }
            try
            {
                foreach (yt_Button button in Mass_Button)
                {
                    CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                             // отменяет отслеживание ошибок,
                                                             // но дает передать компоненты формы в другой поток 
                    button.Click += async (s, e) =>
                    {
                        for (int i = 0; i < MassAll.Length; i++)
                        {
                            if (button == MassAll[i])
                            {
                                await Task.Run(() => Message(button, "🔴"));
                            }
                        }
                        for (int i = 0; i < MassGreen.Length; i++)
                        {
                            if (button == MassGreen[i])
                            {
                                await Task.Run(() => Message(button, button1.Text));
                            }
                        }
                        for (int i = 0; i < MassText.Length; i++)
                        {
                            if (button == MassText[i])
                            {
                                await Task.Run(() => Message(button, ""));
                            }
                        }
                    };

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка в конструкторе в создани событий для кнопок AUD/CAD и далее\n {ex}");
            }
        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SystemEvents.UserPreferenceChanged -= new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
        }


        void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.Window)
            {
                this.Font = SystemFonts.IconTitleFont;
            }
        }

        //Событие для срабатывания кнопки по нажанию на клавишу "Enter"

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter) yt_Button1.PerformClick();
        }

        async public void Message(yt_Button button, string a)
        {
            try
            {
                if (egoldsGoogleTextBox3.Text == "")
                {
                    MessageBox.Show($"Поле для ввода контакта - не заполнено");
                }
                else
                {
                    TelegramClient client_2 = new TelegramClient(apiId, apiHash);
                    await client_2.ConnectAsync();
                    await client_2.SendMessageAsync(new TLInputPeerUser() { UserId = ID }, button.Text + a);
                }
            }
            catch (System.InvalidOperationException)
            {
                MessageBox.Show($"Контакт не найден\nID контакта - неверное, для действующей сессии.\nID контакта = {ID}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }            
        }

        //Событие кнопки подключиться

      async private void yt_Button61_Click(object sender, EventArgs e)
        {
            if(File.Exists("session.dat") & IsCreateSession == false)
            {
                MessageBox.Show($"Файл сессии(session.dat) уже создан\nУдалите его, чтобы создать новую сессию\nРасположен в директории с Send_telegram.exe");
            }
            else
            {
                await Task.Run(() => AuthenticationAsync());
                await Task.Delay(200);
            }            
        }
        //Метод подключения AuthenticationAsync() и создания файла сессии, если файл сессии уже создан,
        //то при нажатии на кнопку "Подключение" сгенерируется ошибка.
        //Кнопка "Подключение" используется только при первом запуске программы,
        //без созданного файла сессии. При работе без файла сессии(при первом запуске программы),
        //необходимо заполнить поле для ввода номера телефона, нажать кнопку подключится,
        //зайти в свой аккаунт телеграмм, проверить, что после нажатия на кнопку "Подключение",
        //пришла капча телеграмма, потом в программу вввести капчу и номер должен быть тоже записан в поле
        //нажать на кнопку "Подключение" еще раз, файл сессии создастся и при последующих запусках программы
        //вводить номер телефона и капчу и нажимать кнопку "Подключение" нельзя,
        //сгенерируется ошибка
        //При дальнейшей работе, чтобы отправить сообщение в телеграмм, неоходимо заполнить поле 
        //"Ввод контакта" и нажать на определенную клавишу и тогда сообщения в телеграмм отправятся.
        //Поле "Ввод контакта" заполняется путем записи имени контакта в поле,
        //контакт должен быть в приложении телеграмм, можно отправлять сообщения самому себе,
        //для этого в поле "Ввод контакта" вводится имя Вашего контакта.

        async public void AuthenticationAsync()
        {
            try
            {
                if (egoldsGoogleTextBox1.Text != "")
                {
                    phoneNumber = egoldsGoogleTextBox1.Text;
                    await client.ConnectAsync();
                    var hash = await client.SendCodeRequestAsync(phoneNumber);
                    var code = egoldsGoogleTextBox2.Text;
                    var user = await client.MakeAuthAsync(phoneNumber, hash, code);
                }
                else
                {
                    MessageBox.Show($"Поле для ввода номера телефона - пустое");
                }
            }
            catch(ArgumentNullException)
            {
                MessageBox.Show($"Введите капчу, отправленную приложением \"Telegram\",\nкоторая пришла в сообщения Вашего зарегистрированного аккаунта, по номеру телефона: {phoneNumber},\nВвод осуществлеяется в текстовое поле \"Capcha input\".\nДля продолжения авторизации, снова нажмите кнопку \"Connect\"");
                IsCreateSession = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: Перезапустите приложение и повторите действия в нем\n{ex}");
            }
        }

        //Событие кнопки сохранить

       async private void yt_Button62_Click(object sender, EventArgs e)
        {
            SaveAsync();
            await Task.Delay(200);
        }
        async public void SaveAsync()
        {
            try
            {
                //string path = @"C:\Users\Eduard.Karpov\source\repos\Send_telegram\Send_telegram\bin\Debug\session.dat";
                //FileInfo m = new FileInfo(path);
                //if(m.Exists)
                //{
                //    File.Delete(path);
                //}            
            TelegramClient client_3 = new TelegramClient(apiId, apiHash);
            await client_3.ConnectAsync();
            SelectionContact(client_3);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }
        async public void SelectionContact(TelegramClient client)
        {
            try
            {
                if (egoldsGoogleTextBox3.Text != "")
                {
                    await client.ConnectAsync();
                    var result = await client.GetContactsAsync();
                    var user = result.Users
                        .Where(x => x.GetType() == typeof(TLUser))
                        .Cast<TLUser>()
                        .FirstOrDefault(x => x.FirstName == egoldsGoogleTextBox3.Text);
                    ID = user.Id;
                }
                else
                    MessageBox.Show($"Поле для ввода имени получателя сообщения - пустое");
            }
            catch (NullReferenceException)
            {
                MessageBox.Show($"Такой пользователь не найден");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }


        //Событие чекбокса Влад

        private void egoldsToggleSwitch1_CheckedChanged(object sender)
        {
            if(egoldsToggleSwitch1.Checked)
            {
                ID = 403672337;
                egoldsGoogleTextBox3.Text = "Ed";
            }
            else
            {
                ID = 0;
                egoldsGoogleTextBox3.Text = "";
            }
        }

        private void egoldsToggleSwitch2_CheckedChanged(object sender)
        {
            Label[] MassLabel = new Label[]
            {
                label1,label11,label12,label2,label3,label10,label16
            };
            yt_Button[] MassText = new yt_Button[8]
      {
                  yt_Button55,
                           yt_Button59,
                                 yt_Button58,
                                   yt_Button57,
                                        yt_Button56,
                                           yt_Button60,
                                           yt_Button61,
                                           yt_Button62,
      };
            if (egoldsToggleSwitch2.Checked)
            {
               
                for (int i = 0; i < MassText.Length; i++)
                {
                   
                    MassText[i].BackColor = System.Drawing.Color.LightSteelBlue;
                }
                for (int i = 0; i < MassLabel.Length; i++)
                {
                    MassLabel[i].BackColor = System.Drawing.Color.Lavender;
                }
                //this.yt_Button55.BackColor = System.Drawing.Color.White;
                //this.yt_Button55.BackColorAdditional = System.Drawing.Color.WhiteSmoke;
                //this.yt_Button55.BackColorGradientEnabled = true;
                //this.yt_Button55.BackColorGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
                //this.yt_Button55.BorderColor = System.Drawing.Color.Gray;
                //this.yt_Button55.BorderColorEnabled = true;
                //this.yt_Button55.BorderColorOnHover = System.Drawing.Color.Maroon;
                //this.yt_Button55.BorderColorOnHoverEnabled = false;
                //egoldsFormStyle1.BackColor = System.Drawing.Color.Black;
            }
            else
            {
                //EgoldsFormStyle style = new EgoldsFormStyle();
                //style.BackColor = System.Drawing.Color.White;
                //egoldsFormStyle1.BackColor = System.Drawing.Color.White;
                for (int i = 0; i < MassText.Length; i++)
                {
                    MassText[i].BackColor = System.Drawing.SystemColors.GradientActiveCaption;
                }
                for (int i = 0; i < MassLabel.Length; i++)
                {
                    MassLabel[i].BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
                }
                //this.yt_Button55.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
                //this.yt_Button55.BackColorAdditional = System.Drawing.Color.WhiteSmoke;
                //this.yt_Button55.BackColorGradientEnabled = true;
                //this.yt_Button55.BackColorGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
                //this.yt_Button55.BorderColor = System.Drawing.Color.Gray;
                //this.yt_Button55.BorderColorEnabled = true;
                //this.yt_Button55.BorderColorOnHover = System.Drawing.Color.Maroon;
                //this.yt_Button55.BorderColorOnHoverEnabled = false;
                //style.StylesDictionary
                //ShadowedForm shadowedForm = new ShadowedForm();
                //shadowedForm.BackColor = System.Drawing.Color.White;
                //Send_telegram form = new Send_telegram();
                //form.BackColor = System.Drawing.Color.White;
            }
        }

        private void egoldsToggleSwitch3_CheckedChanged(object sender)
        {
            Label[] MassLabel = new Label[]
            {
                label1,label11,label12,label2,label3,label10,label16
            };
            yt_Button[] MassText = new yt_Button[8]
     {
                  yt_Button55,
                           yt_Button59,
                                 yt_Button58,
                                   yt_Button57,
                                        yt_Button56,
                                           yt_Button60,
                                           yt_Button61,
                                           yt_Button62,
     };
            if (egoldsToggleSwitch3.Checked)
            {

                for (int i = 0; i < MassText.Length; i++)
                {
                    MassText[i].BackColor = System.Drawing.Color.Gainsboro;
                    MassText[i].BackColorAdditional = System.Drawing.Color.White;
                    //MassText[i].ForeColor = System.Drawing.Color.Gainsboro;
                    //MassText[i].BorderColorEnabled = true;
                    //MassText[i].BorderColorOnHover = System.Drawing.Color.Black;
                    //MassText[i].BorderColor = System.Drawing.Color.Black;
                }
                for (int i = 0; i < MassLabel.Length; i++)
                {
                    MassLabel[i].BackColor = System.Drawing.Color.Lavender;
                    //MassLabel[i].BackColorAdditional = System.Drawing.Color.LightGray;
                    //MassText[i].ForeColor = System.Drawing.Color.Gainsboro;
                    //MassLabel[i].BorderColorEnabled = true;
                    //MassLabel[i].BorderColorOnHover = System.Drawing.Color.Black;
                }
                //this.yt_Button55.BackColor = System.Drawing.Color.RoyalBlue;
                //this.yt_Button55.BackColorAdditional = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
                //this.yt_Button55.ForeColor = System.Drawing.Color.Gainsboro;
            }
            else
            {
                for (int i = 0; i < MassText.Length; i++)
                {
                    MassText[i].BackColor = System.Drawing.SystemColors.GradientActiveCaption;
                    MassText[i].BackColorAdditional = System.Drawing.Color.White;
                    //MassText[i].BorderColorOnHover = System.Drawing.Color.Gray;
                    //MassText[i].BorderColor = System.Drawing.Color.Gray;
                    //MassText[i].BorderColorEnabled = false;
                }
                for (int i = 0; i < MassLabel.Length; i++)
                {
                    MassLabel[i].BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
                }
            }
        }

        bool IsCreateSession = false;

        private void egoldsToggleSwitch4_CheckedChanged(object sender)
        {
            if (egoldsToggleSwitch4.Checked)
                IsCreateSession = true;
            else
                IsCreateSession = false;

        }
    }
}
