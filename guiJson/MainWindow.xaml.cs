using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;
using Forms=System.Windows.Forms;

namespace guiJson
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<jsonModel> listOfJsonModel = new List<jsonModel>();
        public static List<TextBox> ListOfTextBoxAdd = new List<TextBox>();
        public bool editMode = false;
        public string hashCodeOfEditObject = "";
        public MainWindow()
        {
            InitializeComponent();
            initGui();
        }

        /// <summary>
        /// inital the text to specific string 
        /// </summary>
        /// <param name="ListOfTextBox"></param>
        /// <param name="initText"></param>
        private void clearAllTextBoxText(List<TextBox> ListOfTextBox, string initText)
        {
            foreach (var item in ListOfTextBox)
            {
                item.Text = initText;
            }
        }
        /// <summary>
        /// init the gui
        /// </summary>
        private void initGui()
        {
            showMainCanvas();
            refreshObject();
            textBoxAddCanvas.Children.Add(GUIController.createAddMenu(typeof(jsonModel), ref ListOfTextBoxAdd));
        }
        /// <summary>
        /// show main canvas
        /// </summary>
        private void showMainCanvas()
        {
            addObjectCanvas.Visibility = Visibility.Hidden;
            allObjectCanvas.Visibility = Visibility.Visible;
            exitEditMode();
        }
        /// <summary>
        /// exit edit mode
        /// </summary>
        private void exitEditMode()
        {
            editMode = false;
            hashCodeOfEditObject = "";
        }
        /// <summary>
        /// enter to edit mode
        /// </summary>
        /// <param name="editHashObject"></param>
        private void enterEditMode(string editHashObject)
        {
            editMode = true;
            hashCodeOfEditObject = editHashObject;
        }
        /// <summary>
        /// show add canvas 
        /// </summary>
        private void showAddCanvas()
        {
            addObjectCanvas.Visibility = Visibility.Visible;

        }
        /// <summary>
        /// parse the select type from string 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
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

        /// <summary>
        /// refresh main object gui 
        /// </summary>
        public void refreshObject()
        {
            allObjectCanvas.Children.Clear();
            allObjectCanvas.Children.Add(GUIController.createMainObject(this));
        }
        /// <summary>
        /// enter to specific object 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void enterObjectCanvas(object sender, MouseButtonEventArgs e)
        {
            Canvas c = (Canvas)sender;
            enterEditMode(c.Tag + "");
            jsonModel jsonModelChoosen = listOfJsonModel.Find((j) => { return j.GetHashCode() == int.Parse(c.Tag + ""); });
            putInformationToEditScreen(jsonModelChoosen);
            showAddCanvas();
        }
        /// <summary>
        /// put object fields into text box for edit mode   
        /// </summary>
        /// <param name="jsonModel"> </param>
        private void putInformationToEditScreen(object jsonModel)
        {
            FieldInfo[] objectFields = jsonModel.GetType().GetFields();
            foreach (var item in objectFields)
            {
                ListOfTextBoxAdd.Find((t) => { return t.Tag + "" == item.Name; }).Text = item.GetValue(jsonModel) + "";
            }
        }
        /// <summary>
        /// export the objects to json
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportJson_Click(object sender, MouseButtonEventArgs e)
        {
            var json = serializeJson(listOfJsonModel);
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = "Data Json.json";
            save.Filter = "Json File | *.json";
            if (save.ShowDialog() == true)
            {
                StreamWriter writer = new StreamWriter(save.OpenFile());
                    writer.WriteLine(json);
                writer.Dispose();
                writer.Close();
            }
        }
        /// <summary>
        /// serialize list of json model to json string
        /// </summary>
        /// <param name="listOfJsonModelToSerialize">the list you want to serialize</param>
        /// <returns>the list in json string</returns>
        private string serializeJson(List<jsonModel> listOfJsonModelToSerialize)
        {
            return new JavaScriptSerializer().Serialize(listOfJsonModelToSerialize);
        }
        /// <summary>
        /// import json and convert to jsonModel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importJson_Click(object sender, MouseButtonEventArgs e)
        {
            string json = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "json files (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                json = File.ReadAllText(openFileDialog.FileName);
                string message = "do you want to add that to current list? caution: override will delete all your progress! ";
                string caption = "Warning: override the current progress ";

                Forms.MessageBoxButtons buttons = Forms.MessageBoxButtons.YesNo;
                Forms.DialogResult result;

                // Displays the MessageBox.
                result = Forms.MessageBox.Show(message, caption, buttons,Forms.MessageBoxIcon.Warning);
                if (result == Forms.DialogResult.Yes)
                    listOfJsonModel.AddRange(deserializeJson(json));
                else
                    listOfJsonModel = deserializeJson(json);

            }
            refreshObject();
        }
        /// <summary>
        /// desrialize Json to list of json model
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        private List<jsonModel> deserializeJson(string jsonString)
        {
            return new JavaScriptSerializer().Deserialize<List<jsonModel>>(jsonString);
        }
        /// <summary>
        /// save button click
        /// save new object or update exits object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveObject_Click(object sender, MouseButtonEventArgs e)
        {
            saveObject();
            refreshObject();
            showMainCanvas();
        }
        /// <summary>
        /// save a object to list 
        /// </summary>
        private void saveObject()
        {
            jsonModel jsonObjTemp;
            if (editMode)//if we edit exits object
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
        }
        /// <summary>
        /// exit the menu add object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitAddObjectMenu_Click(object sender, MouseButtonEventArgs e)
        {
            showMainCanvas();
        }
        /// <summary>
        /// open the object menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openAddObjectMenu_Click(object sender, MouseButtonEventArgs e)
        {
            clearAllTextBoxText(ListOfTextBoxAdd, "");
            exitEditMode();
            showAddCanvas();
        }
    }

}
