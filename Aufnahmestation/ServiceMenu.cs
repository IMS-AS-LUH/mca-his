using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HisDemo.DTO;

namespace HisDemo.Aufnahmestation
{
    public partial class ServiceMenu : Form
    {
        public ServiceMenu()
        {
            InitializeComponent();
        }

        public enum Action
        {
            None,
            LoadExampleDataset,
            TeachMode,
        }

        public Action ResultAction { get; private set; } = Action.None;

        private void buttonMusterDatensatz_Click(object sender, EventArgs e)
        {
            ResultAction = Action.LoadExampleDataset;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public List<Questionnaire> Questionnaires { get; set; }

        private void buttonQuestionnaireDump_Click(object sender, EventArgs e)
        {
            using(SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.FileName = $"Questionnaire{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace('.','-')}.csv";
                sfd.Filter = "CSV|*.csv";
                sfd.Title = "Save Questionnaire Dump";
                sfd.CheckPathExists = true;
                sfd.OverwritePrompt = true;

                if (sfd.ShowDialog(this) == DialogResult.OK)
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(sfd.FileName, false, Encoding.UTF8))
                    {
                        sw.WriteLine("QID,Questionnaire,Text,ShortText,Type,Required,MustAnswerYes,ShowInPrint,ShowToPatient,Default,Min,Max,DecimalPlaces");
                        
                        foreach(var qn in Questionnaires)
                        {
                            foreach (var q in qn.Questions)
                            {
                                object[] fields =
                                {
                                    q.QID,
                                    qn.Title,
                                    q.QuestionText,
                                    q.ShortText,
                                    q.QuestionType,
                                    q.Required,
                                    q.MustAnswerYes,
                                    qn.ShowInPrint,
                                    qn.ShowToPatient,
                                    q.Default,
                                    q.Minimum,
                                    q.Maximum,
                                    q.DecimalPlaces,

                                };
                                sw.WriteLine(string.Join(",",fields.Select<object, string>(x => $"\"{x?.ToString()?.Replace("\"", "''")?.Replace("\r","")?.Replace("\n"," <br> ") ?? ""}\"")));
                            }
                        }
                    }
                }
            }
        }

        private void buttonTeachMode_Click(object sender, EventArgs e)
        {
            ResultAction = Action.TeachMode;
            this.Close();
        }
    }
}
