
using System;
using System.Collections.Generic;//using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Drawing;
using System.Diagnostics;
using System.Data.SQLite;

namespace SosiDB
{
    class ExcelReport
    {
        FmLogin fmLogin = new FmLogin();
        FmOfferingInput fmOfferingInput = new FmOfferingInput();
        int DrawEnd; //string RemitAmount = "";
        DateTime ReportMonth;
        double TitheSum, ThanksgivingSum1, ThanksgivingSum2, ThanksgivingSum3, F1stFruitSum, PledgeSum, MissionSum, SpecialSum, OtherSum;
        public string ExportToExcel(DataTable dt1, DataTable dt2, string TargetFilename, string ReportMonthYear, string DateRange, string RCCGYear)
        {
            ExcelPackage excelPkg;
            ExcelWorksheet oSheet;
            var tempExcel = new FileInfo(TargetFilename); // Create the file using the FileInfo object
            
            #region Creating excelpkg
            using (excelPkg = new ExcelPackage(tempExcel))
            {
                if (!File.Exists(TargetFilename))
                { // Setting Excel Workbook Properties
                    excelPkg.Workbook.Properties.Author = "AdureX Corp";
                    excelPkg.Workbook.Properties.Title = "SosiDB Report";
                }
                else
                {
                    if (excelPkg.Workbook.Worksheets.Contains(excelPkg.Workbook.Worksheets[ReportMonthYear]))
                        excelPkg.Workbook.Worksheets.Delete(excelPkg.Workbook.Worksheets[ReportMonthYear]);
                }
                // Creating Excel Worksheet
                oSheet = excelPkg.Workbook.Worksheets.Add(ReportMonthYear);
                excelPkg.Workbook.Worksheets.MoveToStart(ReportMonthYear);

                oSheet.Cells.Style.Font.Name = "Arial"; // Setting default font for whole sheet
                oSheet.Cells.Style.Font.Size = 12;
                #region Drawing the cells
                string imagePath = "RCCGLogo.png"; // Add Image in Excel Sheet
                AddImage(oSheet, 0, 0, 120, 100, imagePath);
                // ************************************************
                oSheet.Cells[2, 3].Value = "THE REDEEMED CHRISTIAN CHURCH OF GOD "; oSheet.Cells[2, 3].Style.Font.Bold = true;
                oSheet.Cells[2, 3].Style.Font.Name = "Arial"; oSheet.Cells[2, 3].Style.Font.Size = 24;
                oSheet.Cells[2, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                oSheet.Cells[3, 5].Value = "(AUSTRALIA/PACIFIC REGION) "; oSheet.Cells[3, 5].Style.Font.Bold = true;
                oSheet.Cells[3, 5].Style.Font.Name = "Arial"; oSheet.Cells[3, 5].Style.Font.Size = 16;
                oSheet.Cells[3, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                oSheet.Cells[6, 4].Value = "MONTHLY STATISTICS / FINANCIAL REPORT "; oSheet.Cells[6, 4].Style.Font.Bold = true;
                oSheet.Cells[6, 4].Style.Font.Name = "Arial"; oSheet.Cells[6, 4].Style.Font.Size = 14;
                oSheet.Cells[6, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                oSheet.Cells[8, 1].Value = "ZONE:"; oSheet.Cells[8, 1].Style.Font.Bold = true;
                oSheet.Cells[8, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                oSheet.Cells[8, 2].Value = "ZONE 3"; oSheet.Cells[8, 2].Style.Font.Bold = true;
                oSheet.Cells[8, 2].Style.Font.Name = "Calibri"; oSheet.Cells[8, 2].Style.Font.Size = 12;
                oSheet.Cells[8, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                oSheet.Cells[8, 4].Value = "PARISH:"; oSheet.Cells[8, 4].Style.Font.Bold = true;
                oSheet.Cells[8, 4].Style.Font.Name = "Calibri"; oSheet.Cells[8, 4].Style.Font.Size = 12;
                oSheet.Cells[8, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                oSheet.Cells[8, 5].Value = "FOUNTAIN OF LIFE, CANBERRA";// oSheet.Cells[8, 5].Style.Font.Bold = true;
                oSheet.Cells[8, 5].Style.Font.Name = "Calibri"; oSheet.Cells[8, 5].Style.Font.Size = 12;
                oSheet.Cells[8, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                oSheet.Cells[10, 1].Value = "MONTH:"; oSheet.Cells[10, 1].Style.Font.Bold = true;
                oSheet.Cells[10, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                string RemDate = DateRange;
                oSheet.Cells[10, 2].Value = RemDate; oSheet.Cells[10, 2].Style.Font.Bold = true;
                oSheet.Cells[10, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                // ************************************************************************************
                oSheet.Cells[11, 1].Value = "DATE"; oSheet.Cells[11, 1].Style.Font.Bold = true; oSheet.Cells[11, 1, 12, 1].Merge = true;
                oSheet.Cells[11, 1, 28, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[11, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                oSheet.Cells[11, 2].Value = "SERVICE"; oSheet.Cells[11, 2].Style.Font.Bold = true; oSheet.Cells[11, 2, 12, 2].Merge = true;
                oSheet.Cells[11, 2, 28, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[11, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                oSheet.Cells[11, 3].Value = "PREACHER"; oSheet.Cells[11, 3].Style.Font.Bold = true; oSheet.Cells[11, 3, 12, 3].Merge = true;
                oSheet.Cells[11, 3, 28, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[11, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                oSheet.Cells[11, 3, 28, 3].Style.WrapText = true;

                oSheet.Cells[11, 4].Value = "ATTENDANCE"; oSheet.Cells[11, 4].Style.Font.Bold = true;
                oSheet.Cells[11, 4, 11, 7].Merge = true;
                oSheet.Cells[11, 4, 28, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[11, 4].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                oSheet.Cells[11, 8].Value = "SUNDAY SCHOOL ATTENDANCE"; oSheet.Cells[11, 8].Style.Font.Bold = true;
                oSheet.Cells[11, 8].Style.WrapText = true; oSheet.Cells[11, 8, 12, 8].Merge = true;
                oSheet.Cells[11, 8, 28, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                oSheet.Cells[11, 9].Value = "OFFERING"; oSheet.Cells[11, 9].Style.Font.Bold = true;
                oSheet.Cells[11, 9, 28, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[11, 9].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                oSheet.Cells[11, 10].Value = "TITHE"; oSheet.Cells[11, 10].Style.Font.Bold = true;
                oSheet.Cells[11, 10, 28, 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[11, 10].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                oSheet.Cells[11, 11].Value = "FIRST FRUIT OFFERING"; oSheet.Cells[11, 11].Style.Font.Bold = true;
                oSheet.Cells[11, 11].Style.WrapText = true;
                oSheet.Cells[11, 11, 28, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                oSheet.Cells[11, 12].Value = "THANKSIGIVING/ DONATIONS"; oSheet.Cells[11, 12].Style.Font.Bold = true;
                oSheet.Cells[11, 12].Style.WrapText = true;
                oSheet.Cells[11, 12, 28, 12].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[11, 12].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                oSheet.Cells[11, 13].Value = "MISSIONS"; oSheet.Cells[11, 13].Style.Font.Bold = true;
                oSheet.Cells[11, 13].Style.WrapText = true;
                oSheet.Cells[11, 13, 28, 13].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[11, 13].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                oSheet.Cells[11, 14].Value = "Building Funds"; oSheet.Cells[11, 14].Style.Font.Bold = true;
                oSheet.Cells[11, 14].Style.WrapText = true;
                oSheet.Cells[11, 14, 28, 14].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[11, 14].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                oSheet.Cells[11, 15].Value = "PLEDGES"; oSheet.Cells[11, 15].Style.Font.Bold = true;
                oSheet.Cells[11, 15].Style.WrapText = true;
                oSheet.Cells[11, 15, 28, 15].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[11, 15].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                oSheet.Cells[11, 16].Value = "SPECIAL OFFERINGS"; oSheet.Cells[11, 16].Style.Font.Bold = true;
                oSheet.Cells[11, 16].Style.WrapText = true;
                oSheet.Cells[11, 16, 28, 16].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[11, 16].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                oSheet.Cells[11, 17].Value = "Other Offering/ Funds"; oSheet.Cells[11, 17].Style.Font.Bold = true;
                oSheet.Cells[11, 17].Style.WrapText = true;
                oSheet.Cells[11, 17, 28, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[11, 17].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                oSheet.Cells[11, 18].Value = "TOTAL"; oSheet.Cells[11, 18, 28, 18].Style.Font.Bold = true;
                oSheet.Cells[11, 18, 28, 18].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[11, 18].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                // ********************************************************************************************************
                oSheet.Cells[12, 4].Value = "Men"; oSheet.Cells[12, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[12, 5].Value = "Women"; oSheet.Cells[12, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[12, 6].Value = "Children"; oSheet.Cells[12, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[12, 7].Value = "Total"; oSheet.Cells[12, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                oSheet.Cells[12, 9].Value = "AU$"; oSheet.Cells[12, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[12, 10].Value = "AU$"; oSheet.Cells[12, 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[12, 11].Value = "AU$"; oSheet.Cells[12, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[12, 12].Value = "AU$"; oSheet.Cells[12, 12].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[12, 13].Value = "AU$"; oSheet.Cells[12, 13].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[12, 14].Value = "AU$"; oSheet.Cells[12, 14].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[12, 15].Value = "AU$"; oSheet.Cells[12, 15].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[12, 16].Value = "AU$"; oSheet.Cells[12, 16].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[12, 17].Value = "AU$"; oSheet.Cells[12, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[12, 18].Value = "AU$"; oSheet.Cells[12, 18].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                // ********************************************************************************************************
                oSheet.Cells[28, 1].Value = "TOTAL"; oSheet.Cells[28, 1].Style.Font.Bold = true;
                oSheet.Cells[28, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                oSheet.Column(1).Width = 14.00; oSheet.Column(2).Width = 15.57; oSheet.Column(3).Width = 20.71;
                oSheet.Column(4).Width = 8.43; oSheet.Column(5).Width = 8.43; oSheet.Column(6).Width = 8.90;
                oSheet.Column(7).Width = 8.43; oSheet.Column(8).Width = 17.57;
                oSheet.Column(9).Width = oSheet.Column(10).Width = oSheet.Column(11).Width = 13.43; oSheet.Column(12).Width = 20.71;
                oSheet.Column(13).Width = oSheet.Column(14).Width = oSheet.Column(15).Width = oSheet.Column(16).Width = oSheet.Column(17).Width = 13.43;
                oSheet.Column(18).Width = 16.00;
                // ************ Part 2 ************************************************************************************
                oSheet.Cells[31, 2].Value = "2.  EXPENDITURE/EQUIPMENT PURCHASE"; oSheet.Cells[31, 2].Style.Font.Bold = true;// 
                oSheet.Cells[31, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                oSheet.Cells[32, 1].Value = "DATE"; oSheet.Cells[32, 1].Style.Font.Bold = true; oSheet.Cells[32, 1, 32, 1].Merge = true;
                oSheet.Cells[32, 1, 52, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[32, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                oSheet.Cells[32, 2].Value = "Description"; oSheet.Cells[32, 2].Style.Font.Bold = true;
                for (int i = 32; i < 54; i++) oSheet.Cells[i, 2, i, 7].Merge = true; oSheet.Cells[32, 2, 52, 7].Style.WrapText = true;
                oSheet.Cells[32, 2, 52, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[32, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                oSheet.Cells[32, 8].Value = "Amount(AU$)"; oSheet.Cells[32, 8].Style.Font.Bold = true;
                oSheet.Cells[32, 8, 52, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                oSheet.Cells[53, 1].Value = "TOTAL"; oSheet.Cells[53, 1, 53, 8].Style.Font.Bold = true;
                oSheet.Cells[53, 1, 53, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                oSheet.Cells[53, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                // ************************************************************************************
                oSheet.Cells[13, 2, 28, 2].Style.WrapText = true;
                #endregion
                int rowIndex = 11, Col = dt1.Columns.Count;
                DrawEnd = 29;
                DrawBorder(oSheet, ref rowIndex, ref Col);
                rowIndex = 12;
                WriteCellData1(oSheet, ref rowIndex, dt1);
                FillFooter1(oSheet, dt1);

                rowIndex = 32; Col = dt2.Columns.Count;
                DrawEnd = 54;
                DrawBorder(oSheet, ref rowIndex, ref Col);
                //rowIndex = 33;
                WriteCellData2(oSheet, ref rowIndex, dt2);
                FillFooter2(oSheet, dt2);
                #region Cell Calculations
                oSheet.View.ShowGridLines = false;
                oSheet.Cells[28, 10].Calculate(); TitheSum = double.Parse(fmLogin.TithePercent) / 100 * Convert.ToDouble(oSheet.Cells[28, 10].Value);
                oSheet.Cells[28, 11].Calculate(); F1stFruitSum = double.Parse(fmLogin.F1stFruitPercent) / 100 * Convert.ToDouble(oSheet.Cells[28, 11].Value);
                oSheet.Cells[28, 12].Calculate(); ThanksgivingSum1 = double.Parse(fmLogin.ThanksPercent1) / 100 * Convert.ToDouble(oSheet.Cells[28, 12].Value);
                oSheet.Cells[28, 12].Calculate(); ThanksgivingSum2 = double.Parse(fmLogin.ThanksPercent2) / 100 * Convert.ToDouble(oSheet.Cells[28, 12].Value);
                oSheet.Cells[28, 12].Calculate(); ThanksgivingSum3 = double.Parse(fmLogin.ThanksPercent3) / 100 * Convert.ToDouble(oSheet.Cells[28, 12].Value);
                oSheet.Cells[28, 13].Calculate(); MissionSum = Convert.ToDouble(oSheet.Cells[28, 13].Value);
                                                  OtherSum = 0.00;      // no 14
                oSheet.Cells[28, 15].Calculate(); PledgeSum = Convert.ToDouble(oSheet.Cells[28, 15].Value);
                oSheet.Cells[28, 16].Calculate(); SpecialSum = Convert.ToDouble(oSheet.Cells[28, 16].Value);
                oSheet.Cells[28, 17].Calculate(); Convert.ToDouble(oSheet.Cells[28, 17].Value);//*/
                #endregion
                excelPkg.Save();
            }
             #endregion  
            
            MessageBox.Show("Inside B4 Remit call: ");
            string RemiReturn = ExportRemittance(TargetFilename, ReportMonthYear, RCCGYear);
            return (TitheSum*100/double.Parse(fmLogin.TithePercent)).ToString() + "?" + RemiReturn; 
        }

        private string ExportRemittance(string TargetFilename, string ReportMonthYear, string RCCGYear)
        {
            string RemittanceFileName = fmLogin.PathResource + "Remittance Report" + RCCGYear;
            ReportMonth = DateTime.Parse("01 " + ReportMonthYear);    //DateTime.Now.Month
            int Coln = 0; if (ReportMonth.Month >= 9) Coln = ReportMonth.Month - 4; else Coln = ReportMonth.Month + 8;

            if (!File.Exists(RemittanceFileName))
            {
                using (ExcelPackage excelPkg = new ExcelPackage())// ""
                {
                    // Setting Excel Workbook Properties
                    excelPkg.Workbook.Properties.Author = "AdureX Corp";
                    excelPkg.Workbook.Properties.Title = "SosiDB Report";
                    #region Creating Excel Worksheet
                    ExcelWorksheet oSheet = excelPkg.Workbook.Worksheets.Add("Sheet1");
                    oSheet.Cells.Style.Font.Name = "Arial"; // Setting default font for whole sheet
                    oSheet.Cells.Style.Font.Size = 12;
                    string imagePath = "RCCGLogo.png"; // Add Image in Excel Sheet
                    AddImage(oSheet, 0, 0, 120, 100, imagePath);
                    // ************************************************
                    oSheet.Cells[2, 3].Value = "THE REDEEMED CHRISTIAN CHURCH OF GOD "; oSheet.Cells[2, 3].Style.Font.Bold = true;
                    oSheet.Cells[2, 3].Style.Font.Name = "Arial"; oSheet.Cells[2, 3].Style.Font.Size = 24;
                    oSheet.Cells[2, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    oSheet.Cells[3, 5].Value = "(AUSTRALIA/PACIFIC REGION) "; oSheet.Cells[3, 5].Style.Font.Bold = true;
                    oSheet.Cells[3, 5].Style.Font.Name = "Arial"; oSheet.Cells[3, 5].Style.Font.Size = 16;
                    oSheet.Cells[3, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    //oSheet.Cells[8, 1].Value = "Month: " + ReportMonthYear; oSheet.Cells[8, 1].Style.Font.Bold = true;
                    //oSheet.Cells[8, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    oSheet.Cells[9, 1].Value = "Parish Name: FOUNTAIN OF LIFE, CANBERRA"; oSheet.Cells[9, 1].Style.Font.Bold = true;
                    oSheet.Cells[9, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    oSheet.Cells[10, 1].Value = "Pastor In Charge: Dcn Adura Abiona"; oSheet.Cells[10, 1].Style.Font.Bold = true;
                    oSheet.Cells[10, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    // ************ Part 2 ************************************************************************************
                    oSheet.Cells[11, 1].Value = "S/N"; oSheet.Cells[11, 1].Style.Font.Bold = true; //oSheet.Cells[32, 1, 32, 1].Merge = true;
                    oSheet.Cells[11, 1, 19, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    oSheet.Cells[11, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    oSheet.Cells[11, 2].Value = "Description"; oSheet.Cells[11, 2].Style.Font.Bold = true;
                    for (int i = 11; i < 22; i++) oSheet.Cells[i, 2, i, 4].Merge = true;
                    oSheet.Cells[11, 2, 20, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    oSheet.Cells[11, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    oSheet.Cells[11, 5].Value = "Sept"; oSheet.Cells[11, 5].Style.Font.Bold = true;
                    oSheet.Cells[11, 5, 21, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    oSheet.Cells[11, 6].Value = "Oct"; oSheet.Cells[11, 6].Style.Font.Bold = true;
                    oSheet.Cells[11, 6, 21, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    oSheet.Cells[11, 7].Value = "Nov"; oSheet.Cells[11, 7].Style.Font.Bold = true;
                    oSheet.Cells[11, 7, 21, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    oSheet.Cells[11, 8].Value = "Dec"; oSheet.Cells[11, 8].Style.Font.Bold = true;
                    oSheet.Cells[11, 8, 21, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    oSheet.Cells[11, 9].Value = "Jan"; oSheet.Cells[11, 9].Style.Font.Bold = true;
                    oSheet.Cells[11, 9, 21, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    oSheet.Cells[11, 10].Value = "Feb"; oSheet.Cells[11, 10].Style.Font.Bold = true;
                    oSheet.Cells[11, 10, 21, 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    oSheet.Cells[11, 11].Value = "Mar"; oSheet.Cells[11, 11].Style.Font.Bold = true;
                    oSheet.Cells[11, 11, 21, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    oSheet.Cells[11, 12].Value = "Apr"; oSheet.Cells[11, 12].Style.Font.Bold = true;
                    oSheet.Cells[11, 12, 21, 12].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    oSheet.Cells[11, 13].Value = "May"; oSheet.Cells[11, 13].Style.Font.Bold = true;
                    oSheet.Cells[11, 13, 21, 13].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    oSheet.Cells[11, 14].Value = "Jun"; oSheet.Cells[11, 14].Style.Font.Bold = true;
                    oSheet.Cells[11, 14, 21, 14].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    oSheet.Cells[11, 15].Value = "Jul"; oSheet.Cells[11, 15].Style.Font.Bold = true;
                    oSheet.Cells[11, 15, 21, 15].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    oSheet.Cells[11, 16].Value = "Aug"; oSheet.Cells[11, 16].Style.Font.Bold = true;
                    oSheet.Cells[11, 16, 21, 16].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    oSheet.Cells[11, 17].Value = "TOTAL"; oSheet.Cells[11, 17].Style.Font.Bold = true;
                    oSheet.Cells[11, 17, 21, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    oSheet.Cells[11, 17].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    oSheet.Cells[11, 18].Value = "Remark"; oSheet.Cells[11, 18].Style.Font.Bold = true;
                    oSheet.Cells[11, 18, 21, 18].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    // ************************************************************************************
                    oSheet.Cells[21, 2].Value = "TOTAL"; oSheet.Cells[21, 2].Style.Font.Bold = true;
                    oSheet.Cells[21, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    oSheet.Cells[12, 1].Value = "1"; oSheet.Cells[12, 2].Value = fmLogin.TithePercent + "% of Tithe";
                    oSheet.Cells[13, 1].Value = "2"; oSheet.Cells[13, 2].Value = fmLogin.ThanksPercent1 + "% of Thanksgiving";
                    oSheet.Cells[14, 1].Value = "3"; oSheet.Cells[14, 2].Value = fmLogin.ThanksPercent2 + "% of Thanksgiving";
                    oSheet.Cells[15, 1].Value = "4"; oSheet.Cells[15, 2].Value = fmLogin.ThanksPercent3 + "% of Thanksgiving";
                    oSheet.Cells[16, 1].Value = "5"; oSheet.Cells[16, 2].Value = fmLogin.F1stFruitPercent + "% of First Fruit";
                    oSheet.Cells[17, 1].Value = "6"; oSheet.Cells[17, 2].Value = "Other";
                    oSheet.Cells[18, 2].Value = "Pledges";
                    oSheet.Cells[19, 2].Value = "Special Offerings";
                    oSheet.Cells[20, 2].Value = "Contribution to Missions";

                    oSheet.Column(2).Width = oSheet.Column(3).Width = oSheet.Column(4).Width = 10;
                    oSheet.Column(5).Width = oSheet.Column(6).Width = oSheet.Column(7).Width = oSheet.Column(8).Width = 15;
                    oSheet.Column(9).Width = oSheet.Column(10).Width = oSheet.Column(11).Width = oSheet.Column(12).Width = 15;
                    oSheet.Column(13).Width = oSheet.Column(14).Width = oSheet.Column(15).Width = oSheet.Column(16).Width = 15;
                    oSheet.Column(17).Width = 15; oSheet.Column(18).Width = 20;
                    // ******************************************************************************************
                    #endregion

                    int rowIndex = 11, Col = 18; DrawEnd = 22;
                    DrawBorder(oSheet, ref rowIndex, ref Col);
                    rowIndex = 12; DrawEnd = 20;
                    WriteCellData3(oSheet, ref rowIndex, ref Col);
                    FillFooter3(oSheet, ref Col);

                    oSheet.View.ShowGridLines = false;
                    oSheet.Cells[21, Coln].Calculate();
                    string strReturnx = Convert.ToDouble(oSheet.Cells[21, Coln].Value).ToString();
                   
                    Byte[] content = excelPkg.GetAsByteArray(); // Writting bytes by bytes in Excel File
                    if (File.Exists(RemittanceFileName)) File.Delete(RemittanceFileName);
                    File.WriteAllBytes(RemittanceFileName, content);
                    return strReturnx;
                }
            }
            else
            {
                FileInfo tempExcel = new FileInfo(RemittanceFileName);
                using (ExcelPackage excelPkg = new ExcelPackage(tempExcel))// ""
                {
                    ExcelWorksheet oSheet = excelPkg.Workbook.Worksheets[1];
                    int rowIndex = 11, Col = 18; DrawEnd = 22;
                    rowIndex = 12; DrawEnd = 20;
                    WriteCellData3(oSheet, ref rowIndex, ref Col);
                    FillFooter3(oSheet, ref Col);

                    oSheet.View.ShowGridLines = false;
                    oSheet.Cells[21, Coln].Calculate();
                    string strReturnx = Convert.ToDouble(oSheet.Cells[21, Coln].Value).ToString();//*/
                    excelPkg.Save();
                    return strReturnx;
                }
            }
        }

        private void DrawBorder(ExcelWorksheet oSheet, ref int rowIndex, ref int Col)
        {
            for (int i = rowIndex; i < DrawEnd; i++)
            {
                for (int colIndex = 1; colIndex <= Col; colIndex++)
                {
                    var cell = oSheet.Cells[i, colIndex];
                    // Setting top/left, right/bottom border of header cells
                    var border = cell.Style.Border;
                    border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }
            }
        }
        private void WriteCellData1(ExcelWorksheet oSheet, ref int rowIndex, DataTable dt1)
        {
            int colIndex = 0;
            foreach (DataRow dr in dt1.Rows)
            {
                colIndex = 1;
                rowIndex++;
                foreach (DataColumn dc in dt1.Columns)
                {
                    var cell = oSheet.Cells[rowIndex, colIndex];
                    // Setting value in the cell
                    if (colIndex == 1) { cell.Value = Convert.ToDateTime(dr[dc.ColumnName]); cell.Style.Numberformat.Format = "dd/MM/yyyy"; }
                    if (colIndex == 2 || colIndex == 3) cell.Value = Convert.ToString(dr[dc.ColumnName]);
                    if (colIndex >= 4 && colIndex <= 8) { cell.Value = Convert.ToInt32(dr[dc.ColumnName]); cell.Style.Numberformat.Format = "#,###,##0"; } // Setting integer format
                    if (colIndex >= 9 && colIndex <= 15) { cell.Value = Convert.ToDouble(dr[dc.ColumnName]); cell.Style.Numberformat.Format = "#,###,##0.00"; } // Setting decimal format  
                    colIndex++;
                }
            }
        }
        private void WriteCellData2(ExcelWorksheet oSheet, ref int rowIndex, DataTable dt2)
        {
            int colIndex = 0;
            foreach (DataRow dr in dt2.Rows)
            {
                colIndex = 1;
                rowIndex++;
                foreach (DataColumn dc in dt2.Columns)
                {
                    var cell = oSheet.Cells[rowIndex, colIndex];
                    // Setting value in the cell
                    if (colIndex == 1) { cell.Value = Convert.ToDateTime(dr[dc.ColumnName]); cell.Style.Numberformat.Format = "dd/MM/yyyy"; }
                    if (colIndex == 2) cell.Value = Convert.ToString(dr[dc.ColumnName]);
                    if (colIndex == 8) { cell.Value = -1*Convert.ToDouble(dr[dc.ColumnName]); cell.Style.Numberformat.Format = "#,###,##0.00"; } // Setting decimal format                   
                    colIndex++;
                }
            }
        }
        private void WriteCellData3(ExcelWorksheet oSheet, ref int rowIndex, ref int Col)
        {
            int Colmx = 0; int Monthx = ReportMonth.Month;// DateTime.Now.Month;
            if (Monthx >= 9) Colmx = Monthx - 4;
            else Colmx = Monthx + 8;

            for (int i = rowIndex; i <= DrawEnd; i++)
            {

                for (int colIndex = 1; colIndex <= Col; colIndex++)
                {
                    var cell = oSheet.Cells[i, colIndex];
                    // Setting value in the cell
                    if (colIndex == Colmx)
                    {
                        if (i == 12) cell.Value = TitheSum;
                        if (i == 13) cell.Value = ThanksgivingSum1;
                        if (i == 14) cell.Value = ThanksgivingSum2;
                        if (i == 15) cell.Value = ThanksgivingSum3;
                        if (i == 16) cell.Value = F1stFruitSum;
                        if (i == 17) cell.Value = OtherSum;
                        if (i == 18) cell.Value = PledgeSum;
                        if (i == 19) cell.Value = SpecialSum;
                        if (i == 20) cell.Value = MissionSum;
                        cell.Style.Numberformat.Format = "#,###,##0.00"; // Setting decimal format 
                    }
                }
            }
        }

        private void FillFooter1(ExcelWorksheet oSheet, DataTable dt)
        {
            int colIndex = 1;
            foreach (DataColumn dc in dt.Columns)
            {
                var cell = oSheet.Cells[28, colIndex];
                if (colIndex >= 4) cell.Formula = "Sum(" + oSheet.Cells[13, colIndex].Address + ":" + oSheet.Cells[27, colIndex].Address + ")";

                if (colIndex >= 4 && colIndex <= 8) cell.Style.Numberformat.Format = "#,###,##0";// Setting integer format
                else if (colIndex >= 9 && colIndex <= 17) cell.Style.Numberformat.Format = "#,###,##0.00"; // Setting decimal format 
                // Setting Background Fill color to Gray
                cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Color.Gray);
                colIndex++;
            }

            for (int rowIndex = 13; rowIndex <= 28; rowIndex++)
            {
                var cell = oSheet.Cells[rowIndex, 18];
                    cell.Formula = "Sum(" + oSheet.Cells[rowIndex, 9].Address + ":" + oSheet.Cells[rowIndex, 17].Address + ")";
                    cell.Style.Numberformat.Format = "#,###,##0.00"; // Setting decimal format
            }
        }
        private void FillFooter2(ExcelWorksheet oSheet, DataTable dt)
        {
            int colIndex = 1;
            foreach (DataColumn dc in dt.Columns)
            {
                var cell = oSheet.Cells[53, colIndex];
                if (colIndex == 8) cell.Formula = "Sum(" + oSheet.Cells[33, colIndex].Address + ":" + oSheet.Cells[52, colIndex].Address + ")";

                if (colIndex == 8) cell.Style.Numberformat.Format = "#,###,##0.00"; // Setting decimal format 
                // Setting Background Fill color to Gray
                cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Color.Gray);
                colIndex++;
            }
        }
        private void FillFooter3(ExcelWorksheet oSheet, ref int Col)
        {
            for (int colIndex = 2; colIndex <= Col - 1; colIndex++)
            {
                var cell = oSheet.Cells[21, colIndex];
                if (colIndex >= 5 && colIndex <= 17)
                {
                    cell.Formula = "Sum(" + oSheet.Cells[12, colIndex].Address + ":" + oSheet.Cells[20, colIndex].Address + ")";
                    cell.Style.Numberformat.Format = "#,###,##0.00"; // Setting decimal format 
                }
                // Setting Background Fill color to Gray
                cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Color.Gray);
            }
        }

        private void AddImage(ExcelWorksheet oSheet, int rowIndex, int colIndex, int PixelWidth, int PixelHeight, string imagePath)
        {
            Bitmap image = new Bitmap(imagePath);
            ExcelPicture excelImage = null;
            if (image != null)
            {
                excelImage = oSheet.Drawings.AddPicture("RCCG Logo", image);
                excelImage.From.Column = colIndex;
                excelImage.From.Row = rowIndex;
                excelImage.SetSize(PixelWidth, PixelHeight);// (100, 100);
                // 2x2 px space for better alignment
                excelImage.From.ColumnOff = Pixel2MTU(10);
                excelImage.From.RowOff = Pixel2MTU(10);
            }
        }
        private int Pixel2MTU(int pixels)
        {
            int mtus = pixels * 9525;
            return mtus;
        }

        // *******************  Load Excel to Data table ****************************
        public DataTable GetTableFromExcel(string PathFilename, DateTime DateFrom, DateTime DateTo, bool addPreviousRow)
        {
            using (var pck = new ExcelPackage())
            {
                string FileExt = Path.GetExtension(PathFilename);
                ExcelWorksheet ws;
                if (FileExt == ".csv")
                {
                    var format = new ExcelTextFormat();
                    format.Delimiter = ',';
                    format.Culture = new System.Globalization.CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
                    format.Culture.DateTimeFormat.ShortDatePattern = "dd/mm/yyyy";
                    format.EOL = "\r";  //DEFAULT IS "\r\n";             
                    format.TextQualifier = '"';
                    ws = pck.Workbook.Worksheets.Add("Dummy");
                    ws.Cells["A1"].LoadFromText(new FileInfo(PathFilename), format, OfficeOpenXml.Table.TableStyles.Medium27, false);
                }
                else
                {
                    using (var stream = File.OpenRead(PathFilename)) { pck.Load(stream); }
                    ws = pck.Workbook.Worksheets.First();
                }

                DataTable tbl = new DataTable(); bool stopPreviousRow = true;
                tbl.Columns.Add("Date"); tbl.Columns.Add("Amount"); tbl.Columns.Add("Descriptions"); tbl.Columns.Add("Balance");
                for (int rowNum = 1; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    DateTime DateCell; DataRow row;
                    if (DateTime.TryParse(ws.Cells[rowNum, 1, rowNum, 1].Text, out DateCell))
                    {
                        if (DateCell.Date >= DateFrom.Date && DateCell.Date <= DateTo.Date)
                        {
                            var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                            row = tbl.Rows.Add(); //int i = 0;
                            foreach (var cell in wsRow)row[cell.Start.Column - 1] = cell.Text;
                        }
                        else
                        {
                            if (addPreviousRow == true && DateCell.Date < DateFrom.Date && stopPreviousRow == true)
                            {
                                var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                                row = tbl.Rows.Add(); 
                                foreach (var cell in wsRow) row[cell.Start.Column - 1] = cell.Text;
                                stopPreviousRow = false;
                            }
                        }
                    }
                }
                tbl.Columns["Balance"].SetOrdinal(2); tbl.Columns["Descriptions"].SetOrdinal(3);
                //tbl.Columns.Add("Accounts"); tbl.Columns.Add("Offering");
                return tbl;
            }
        }

        public string GetRepotDateFromExcel(string PathFilename, string MonthYear)
        {
            string strReturn = "";
            using (var pck = new ExcelPackage())
            {
                using (var stream = File.OpenRead(PathFilename)) { pck.Load(stream); }
                ExcelWorksheet ws = pck.Workbook.Worksheets.First();
                for (int rowNum = 1; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    if (rowNum > 1)
                    {
                        if (MonthYear == DateTime.Parse(ws.Cells[rowNum, 1, rowNum, 1].Value.ToString()).ToString("MMM-yy"))
                        {
                            //MessageBox.Show("Report Month: " + DateTime.Parse(ws.Cells[rowNum, 1, rowNum, 1].Value.ToString()).ToString("MMM-yy"));
                            strReturn = ws.Cells[rowNum, 2, rowNum, 2].Value.ToString() + ";" + ws.Cells[rowNum + 1, 3, rowNum + 1, 3].Value.ToString();
                        }
                    }
                }             
            }
            strReturn = strReturn.Replace("st", ""); strReturn = strReturn.Replace("nd", "");
            strReturn = strReturn.Replace("rd", ""); strReturn = strReturn.Replace("th", "");
            return strReturn;
        }
  // *************** End ****************************
    }
}
