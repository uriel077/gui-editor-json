using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;

namespace guiJson
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<jsonModel> listOfJsonModel;
        public static List<TextBox> ListOfTextBoxAdd;
        public bool editMode = false;
        public string hashCodeOfEditObject = "";
        public MainWindow()
        {
            initVaribles();
            InitializeComponent();
            initGui();
        }
        private void initVaribles()
        {
            listOfJsonModel = new List<jsonModel>();
            ListOfTextBoxAdd = new List<TextBox>();
        }
      
        private void clearAllTextBoxText(List<TextBox> ListOfTextBox)
        {
            foreach (var item in ListOfTextBox)
            {
                item.Text = "";
            }
        }
        private void initGui()
        {
            showMainCanvas();
            refreshObject();
            textBoxAddCanvas.Children.Add(GUIController.createAddObject(typeof(jsonModel), ref ListOfTextBoxAdd));
        }
        private void showMainCanvas()
        {
            addObjectCanvas.Visibility = Visibility.Hidden;
            allObjectCanvas.Visibility = Visibility.Visible;
            exitEditMode();
        }
        private void exitEditMode()
        {
            editMode = false;
            hashCodeOfEditObject = "";
        }
        private void enterEditMode(string editHashObject)
        {
            editMode = true;
            hashCodeOfEditObject = editHashObject;
        }
        private void showAddCanvas()
        {
            addObjectCanvas.Visibility = Visibility.Visible;

        }
       /* private void extractson_Click(object sender, RoutedEventArgs e)
        {
            var json = new JavaScriptSerializer().Serialize(listOfJsonModel);
            MessageBox.Show(json);
        }*/

      /*  private void saveObject_Click(object sender, RoutedEventArgs e)
        {
            jsonModel jsonObjTemp;
            if (editMode)

                jsonObjTemp = listOfJsonModel.Find((j) => { return j.GetHashCode() == int.Parse(hashCodeOfEditObject); });
            else
                jsonObjTemp = new jsonModel();
            foreach (var item in ListOfTextBoxAdd)
            {
                dynamic valueToAdd;
                FieldInfo fieldInfo = jsonObjTemp.GetType().GetField(item.Tag + "");
                valueToAdd = parseSelect(fieldInfo.FieldType, item.Text);
                fieldInfo.SetValue(jsonObjTemp, valueToAdd);
            }
            if (!editMode)
                listOfJsonModel.Add(jsonObjTemp);
            refreshObject();
            showMainCanvas();
        }*/
        private dynamic parseSelect(Type type, string value)
        {

            try
            {
                switch (type.Name)
                {
                    case "Boolean":
                        return bool.Parse(value);

                    case "Byte":
                        return byte.Parse(value);

                    case "Char":
                        return char.Parse(value);

                    case "DateTime":
                        return DateTime.Parse(value);

                    case "DBNull":
                        return DBNull.Value;

                    case "Decimal":
                        return decimal.Parse(value);

                    case "Double":
                        return double.Parse(value);

                    case "Empty":
                        return null;

                    case "Int16":
                        return int.Parse(value);

                    case "Int32":
                        return int.Parse(value);

                    case "Int64":
                        return int.Parse(value);

                    case "Object":
                        return value;

                    case "SByte":
                        return sbyte.Parse(value);

                    case "Single":
                        return Single.Parse(value);

                    case "String":
                        return value;

                    case "UInt16":
                        return uint.Parse(value);

                    case "UInt32":
                        return uint.Parse(value);

                    case "UInt64":
                        return uint.Parse(value);
                }
            }
            catch (Exception)
            {


            }


            return null;
        }

        public void refreshObject()
        {
            allObjectCanvas.Children.Clear();
            allObjectCanvas.Children.Add(GUIController.createMainObject(this));

        }

      /*  private void importJson_Copy_Click(object sender, RoutedEventArgs e)
        {
            string fileLoc = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "json files (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                fileLoc = File.ReadAllText(openFileDialog.FileName);
                string message = "do you want to add that to current list? cutaion: override will overide all your progress! ";
                string caption = "Error Detected in Input";

                System.Windows.Forms.MessageBoxButtons buttons = System.Windows.Forms.MessageBoxButtons.YesNo;
                System.Windows.Forms.DialogResult result;

                // Displays the MessageBox.
                result = System.Windows.Forms.MessageBox.Show(message, caption, buttons);
                if (result == System.Windows.Forms.DialogResult.Yes)
                    listOfJsonModel.AddRange(new JavaScriptSerializer().Deserialize<List<jsonModel>>(fileLoc));
                else
                    listOfJsonModel = new JavaScriptSerializer().Deserialize<List<jsonModel>>(fileLoc);

            }
            refreshObject();
        }*/

       
        public void enterObjectCanvas(object sender, MouseButtonEventArgs e)
        {
            Canvas c = (Canvas)sender;
            enterEditMode(c.Tag + "");
            jsonModel jsonModelChoosen = listOfJsonModel.Find((j) => { return j.GetHashCode() == int.Parse(c.Tag + ""); });
            putInformationToEditScreen(jsonModelChoosen);
            showAddCanvas();
        }

        private void putInformationToEditScreen(object jsonModel)
        {
            foreach (var item in jsonModel.GetType().GetFields())
            {
                ListOfTextBoxAdd.Find((t) => { return t.Tag + "" == item.Name; }).Text = item.GetValue(jsonModel) + "";
            }
        }

        private void extractson_Click(object sender, MouseButtonEventArgs e)
        {
            var json = new JavaScriptSerializer().Serialize(listOfJsonModel);
            MessageBox.Show(json);
        }

        private void importJson_Click(object sender, MouseButtonEventArgs e)
        {
            string fileLoc = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "json files (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                fileLoc = File.ReadAllText(openFileDialog.FileName);
                string message = "do you want to add that to current list? cutaion: override will overide all your progress! ";
                string caption = "Error Detected in Input";

                System.Windows.Forms.MessageBoxButtons buttons = System.Windows.Forms.MessageBoxButtons.YesNo;
                System.Windows.Forms.DialogResult result;

                // Displays the MessageBox.
                result = System.Windows.Forms.MessageBox.Show(message, caption, buttons);
                if (result == System.Windows.Forms.DialogResult.Yes)
                    listOfJsonModel.AddRange(new JavaScriptSerializer().Deserialize<List<jsonModel>>(fileLoc));
                else
                    listOfJsonModel = new JavaScriptSerializer().Deserialize<List<jsonModel>>(fileLoc);

            }
            refreshObject();
        }

        private void saveObject_Click(object sender, MouseButtonEventArgs e)
        {
            jsonModel jsonObjTemp;
            if (editMode)

                jsonObjTemp = listOfJsonModel.Find((j) => { return j.GetHashCode() == int.Parse(hashCodeOfEditObject); });
            else
                jsonObjTemp = new jsonModel();
            foreach (var item in ListOfTextBoxAdd)
            {
                dynamic valueToAdd;
                FieldInfo fieldInfo = jsonObjTemp.GetType().GetField(item.Tag + "");
                valueToAdd = parseSelect(fieldInfo.FieldType, item.Text);
                fieldInfo.SetValue(jsonObjTemp, valueToAdd);
            }
            if (!editMode)
                listOfJsonModel.Add(jsonObjTemp);
            refreshObject();
            showMainCanvas();
        }

        private void exitAddObject_Click(object sender, MouseButtonEventArgs e)
        {
            showMainCanvas();
        }

        private void addObject_Click(object sender, MouseButtonEventArgs e)
        {
            clearAllTextBoxText(ListOfTextBoxAdd);
            exitEditMode();
            showAddCanvas();
        }
    }

}
