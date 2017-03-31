using System;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Forms;
using PdfSharp.Drawing.Layout;
using System.IO;

namespace SosiDB
{
    /// <summary>
    /// Represents a dialog containing a <see cref="CoolPrintPreviewControl"/> control
    /// used to preview and print <see cref="PrintDocument"/> objects.
    /// </summary>
    /// <remarks>
    /// This dialog is similar to the standard <see cref="PrintPreviewDialog"/>
    /// but provides additional options such printer and page setup buttons,
    /// a better UI based on the <see cref="ToolStrip"/> control, and built-in
    /// PDF export.
    /// </remarks>
    internal partial class PrintPreviewXDialog : Form
    {
        //--------------------------------------------------------------------
        #region ** fields

        PrintDocument _doc;

        #endregion

        //--------------------------------------------------------------------
        #region ** ctor

        /// <summary>
        /// Initializes a new instance of a <see cref="CoolPrintPreviewDialog"/>.
        /// </summary>
        public PrintPreviewXDialog()
            : this(null)
        {
        }
        /// <summary>
        /// Initializes a new instance of a <see cref="CoolPrintPreviewDialog"/>.
        /// </summary>
        /// <param name="parentForm">Parent form that defines the initial size for this dialog.</param>
        public PrintPreviewXDialog(Control parentForm)
        {
            InitializeComponent();
            if (parentForm != null)
            {
                Size = parentForm.Size;
            }
        }
        #endregion

        //--------------------------------------------------------------------
        #region ** object model

        /// <summary>
        /// Gets or sets the <see cref="PrintDocument"/> to preview.
        /// </summary>
        public PrintDocument Document
        {
            get { return _doc; }
            set
            {
                // unhook event handlers
                if (_doc != null)
                {
                    _doc.BeginPrint -= _doc_BeginPrint;
                    _doc.EndPrint -= _doc_EndPrint;
                }

                // save the value
                _doc = value;

                // hook up event handlers
                if (_doc != null)
                {
                    _doc.BeginPrint += _doc_BeginPrint;
                    _doc.EndPrint += _doc_EndPrint;
                }


                // don't assign document to preview until this form becomes visible
                if (Visible)
                {
                    _preview.Document = Document;
                }
            }
        }

        #endregion

        //--------------------------------------------------------------------
        #region ** overloads

        /// <summary>
        /// Overridden to assign document to preview control only after the 
        /// initial activation.
        /// </summary>
        /// <param name="e"><see cref="EventArgs"/> that contains the event data.</param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _preview.Document = Document;
        }
        /// <summary>
        /// Overridden to cancel any ongoing previews when closing form.
        /// </summary>
        /// <param name="e"><see cref="FormClosingEventArgs"/> that contains the event data.</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (_preview.IsRendering && !e.Cancel)
            {
                _preview.Cancel(); //_preview.can
            }
        }

        #endregion

        //--------------------------------------------------------------------
        #region ** main commands
        void _btnPrint_Click(object sender, EventArgs e)
        {
            using (var dlg = new PrintDialog())
            {
                // configure dialog
                dlg.AllowSomePages = true;
                dlg.AllowSelection = true;
                dlg.UseEXDialog = true;
                dlg.Document = Document;

                // show allowed page range
                var ps = dlg.PrinterSettings;
                ps.MinimumPage = ps.FromPage = 1;
                ps.MaximumPage = ps.ToPage = _preview.PageCount;

                // show dialog
                if (dlg.ShowDialog(this) == DialogResult.OK) _preview.Print();
            }
        }
        void _btnPageSetup_Click(object sender, EventArgs e)
        {
            using (var dlg = new PageSetupDialog())
            {
                dlg.Document = Document;
                if (dlg.ShowDialog(this) == DialogResult.OK) _preview.RefreshPreview();
            }
        }
        void _btnPDF_Click(object sender, EventArgs e)
        {
            DocNamex = fmLogin.PathResource + Document.DocumentName + ".pdf";
            Process.Start(DocNamex);
        }

        #endregion
        //--------------------------------------------------------------------
        #region ** zoom

        void _btnZoom_ButtonClick(object sender, EventArgs e)
        {
            _preview.ZoomMode = _preview.ZoomMode == ZoomMode.ActualSize
                ? ZoomMode.FullPage
                : ZoomMode.ActualSize;
        }
        void _btnZoom_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == _itemActualSize)
            {
                _preview.ZoomMode = ZoomMode.ActualSize;
            }
            else if (e.ClickedItem == _itemFullPage)
            {
                _preview.ZoomMode = ZoomMode.FullPage;
            }
            else if (e.ClickedItem == _itemPageWidth)
            {
                _preview.ZoomMode = ZoomMode.PageWidth;
            }
            else if (e.ClickedItem == _itemTwoPages)
            {
                _preview.ZoomMode = ZoomMode.TwoPages;
            }
            if (e.ClickedItem == _item10)
            {
                _preview.Zoom = .1;
            }
            else if (e.ClickedItem == _item100)
            {
                _preview.Zoom = 1;
            }
            else if (e.ClickedItem == _item150)
            {
                _preview.Zoom = 1.5;
            }
            else if (e.ClickedItem == _item200)
            {
                _preview.Zoom = 2;
            }
            else if (e.ClickedItem == _item25)
            {
                _preview.Zoom = .25;
            }
            else if (e.ClickedItem == _item50)
            {
                _preview.Zoom = .5;
            }
            else if (e.ClickedItem == _item500)
            {
                _preview.Zoom = 5;
            }
            else if (e.ClickedItem == _item75)
            {
                _preview.Zoom = .75;
            }
        }
        #endregion
        //--------------------------------------------------------------------
        #region ** page navigation

        void _btnFirst_Click(object sender, EventArgs e)
        {
            _preview.StartPage = 0;
        }
        void _btnPrev_Click(object sender, EventArgs e)
        {
            _preview.StartPage--;
        }
        void _btnNext_Click(object sender, EventArgs e)
        {
            _preview.StartPage++;
        }
        void _btnLast_Click(object sender, EventArgs e)
        {
            _preview.StartPage = _preview.PageCount - 1;
        }
        void _txtStartPage_Enter(object sender, EventArgs e)
        {
            _txtStartPage.SelectAll();
        }
        void _txtStartPage_Validating(object sender, CancelEventArgs e)
        {
            CommitPageNumber();
        }
        void _txtStartPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            var c = e.KeyChar;
            if (c == (char)13)
            {
                CommitPageNumber();
                e.Handled = true;
            }
            else if (c > ' ' && !char.IsDigit(c))
            {
                e.Handled = true;
            }
        }
        void CommitPageNumber()
        {
            int page;
            if (int.TryParse(_txtStartPage.Text, out page))
            {
                _preview.StartPage = page - 1;
            }
        }
        void _preview_StartPageChanged(object sender, EventArgs e)
        {
            var page = _preview.StartPage + 1;
            _txtStartPage.Text = page.ToString();
        }
        private void _preview_PageCountChanged(object sender, EventArgs e)
        {
            this.Update();
            Application.DoEvents();
            _lblPageCount.Text = string.Format("of {0}", _preview.PageCount);
        }

        #endregion
        //--------------------------------------------------------------------
        #region ** job control

        void _btnCancel_Click(object sender, EventArgs e)
        {
            if (_preview.IsRendering)
            {
                _preview.Cancel();
            }
            else
            {
                Close();
            }
        }
        void _doc_BeginPrint(object sender, PrintEventArgs e)
        {
            _btnCancel.Text = "&Cancel";
            _btnPrint.Enabled = _btnPageSetup.Enabled = false;
        }
        void _doc_EndPrint(object sender, PrintEventArgs e)
        {
            _btnCancel.Text = "&Close";
            _btnPrint.Enabled = _btnPageSetup.Enabled = true;
        }

        #endregion
        private void PrintView()
        {
            using (var dlg = new PrintPreviewDialog())
            {
                dlg.Document = this.PrintDocReg;
                dlg.ShowDialog();
            }
        }
        private void PrintViewX()
        {
            using (var dlg = new PrintPreviewXDialog())
            {
                dlg.Document = this.PrintDocReg;
                dlg.ShowDialog();
            }
        }

        #region %%% Print DataGridView Member Variables %%%%%%%%%
        StringFormat strFormat; //Used to format the grid rows.
        ArrayList arrColumnLefts = new ArrayList();//Used to save left coordinates of columns
        ArrayList arrColumnWidths = new ArrayList();//Used to save column widths
        int iCellHeight = 0; //Used to get/set the datagridview cell height
        int iTotalWidth = 0; //
        int iRow = 0;//Used as counter
        bool bFirstPage = false; //Used to check whether we are printing first page
        bool bNewPage = false;// Used to check whether we are printing a new page
        int iHeaderHeight = 0; //Used for the header height
        string AdHeader = "Customer Summary", DocNamex; // Document header title
        int XMrgBdLeft = 10, XMrgBdTop = 10, XMrgBdWidth = 10, XMrgBdHeight = 10;
        DataGridView dgvRegister;
        PdfDocument document;
        FmLogin fmLogin = new FmLogin();
        bool OrienLandscape = false;
        #endregion
        //------------------------------------------------------
        #region Begin Print Event Handler: Handles the begin print event of print document
        public void PrintDGV(DataGridView DGV, string Header, string DocName, bool OrienLandscapex)
        {
            if (Header != "") AdHeader = Header;
            DocNamex = fmLogin.PathResource + DocName + ".pdf";
            OrienLandscape = OrienLandscapex;

            this.PrintDocReg.DocumentName = DocName;
            this.PrintDocReg.DefaultPageSettings.Landscape = OrienLandscapex; if (OrienLandscapex == true)
            {
                Margins margins = new Margins(150, 150, 300, 300);
                this.PrintDocReg.DefaultPageSettings.Margins = margins;
            }
            PrintView();
        }
        public void PrintDGVX(DataGridView DGV, string Header, string DocName, bool OrienLandscapex)
        {
            dgvRegister = DGV;
            this.Text = dgvRegister.ToString();
            if (Header != "") AdHeader = Header;
            OrienLandscape = OrienLandscapex;

            this.PrintDocReg.DocumentName = DocName;
            this.PrintDocReg.DefaultPageSettings.Landscape = OrienLandscapex; if (OrienLandscapex == true)
            {
                Margins margins = new Margins(150, 150, 300, 300);
                this.PrintDocReg.DefaultPageSettings.Margins = margins;
            }
            PrintViewX();
        }
        public void DGVPdf(DataGridView DGV, string Header, string DocName, bool OrienLandscapex)
        {
            dgvRegister = DGV;
            if (Header != "") AdHeader = Header;
            DocNamex = fmLogin.PathResource + DocName + ".pdf";
            OrienLandscape = OrienLandscapex;
            PDFRender();
            document.Close();
            Process.Start(DocNamex);
        }
        public void DGVPdfPrint(DataGridView DGV, string Header, string DocName, bool OrienLandscapex)
        {
            dgvRegister = DGV;
            if (Header != "") AdHeader = Header;
            DocNamex = fmLogin.PathResource + DocName + ".pdf";
            OrienLandscape = OrienLandscapex;
            PDFRender();
            document.Close();
        }
        private void PrintDocReg_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                strFormat = new StringFormat();
                strFormat.Alignment = StringAlignment.Near;
                strFormat.LineAlignment = StringAlignment.Center;
                strFormat.Trimming = StringTrimming.EllipsisCharacter;

                arrColumnLefts.Clear();
                arrColumnWidths.Clear();
                iCellHeight = 0;
                iRow = 0;
                bFirstPage = true;
                bNewPage = true;

                // Calculating Total Widths
                iTotalWidth = 0;
                foreach (DataGridViewColumn dgvGridCol in dgvRegister.Columns)
                {
                    if (dgvGridCol.Visible == true) iTotalWidth += dgvGridCol.Width;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// Handles the print page event of print document
        private void PrintDocReg_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                //Set the left margin
                int iLeftMargin = e.MarginBounds.Left;
                //Set the top margin
                int iTopMargin = e.MarginBounds.Top;
                //Whether more pages have to print or not
                bool bMorePagesToPrint = false;
                int iTmpWidth = 0;

                //For the first page to print set the cell width and header height
                if (bFirstPage)
                {
                    foreach (DataGridViewColumn GridCol in dgvRegister.Columns)
                    {
                        if (GridCol.Visible == true)
                        {
                            iTmpWidth = (int)(Math.Floor((double)((double)GridCol.Width /
                                           (double)iTotalWidth * (double)iTotalWidth *
                                           ((double)e.MarginBounds.Width / (double)iTotalWidth))));

                            iHeaderHeight = dgvRegister.ColumnHeadersHeight;

                            // Save width and height of headers
                            arrColumnLefts.Add(iLeftMargin);
                            arrColumnWidths.Add(iTmpWidth);
                            iLeftMargin += iTmpWidth;
                        }
                    }
                }
                //Loop till all the grid rows not get printed
                while (iRow <= dgvRegister.Rows.Count - 1)
                {
                    DataGridViewRow GridRow = dgvRegister.Rows[iRow];
                    //Set the cell height
                    iCellHeight = GridRow.Height + 18;
                    int iCount = 0;
                    //Check whether the current page settings allow more rows to print
                    if (iTopMargin + iCellHeight >= e.MarginBounds.Height + e.MarginBounds.Top)
                    {
                        bNewPage = true;
                        bFirstPage = false;
                        bMorePagesToPrint = true;
                        break;
                    }
                    else
                    {
                        if (bNewPage)
                        {
                            //Draw Header
                            e.Graphics.DrawString(AdHeader, new System.Drawing.Font(dgvRegister.Font, FontStyle.Bold),
                                    Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top -
                                    e.Graphics.MeasureString(AdHeader, new System.Drawing.Font(dgvRegister.Font, FontStyle.Bold),
                                    e.MarginBounds.Width).Height - 13);

                            //Draw Date
                            String strDate = "Printed by SosiDB software: " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString();
                            e.Graphics.DrawString(strDate, new System.Drawing.Font(dgvRegister.Font, FontStyle.Bold),
                                    Brushes.Black, e.MarginBounds.Left + (e.MarginBounds.Width -
                                    e.Graphics.MeasureString(strDate, new System.Drawing.Font(dgvRegister.Font,
                                    FontStyle.Bold), e.MarginBounds.Width).Width), e.MarginBounds.Top -
                                    e.Graphics.MeasureString(AdHeader, new System.Drawing.Font(new System.Drawing.Font(dgvRegister.Font,
                                    FontStyle.Bold), FontStyle.Bold), e.MarginBounds.Width).Height - 13);

                            //Draw Column headers                 
                            iTopMargin = e.MarginBounds.Top;
                            foreach (DataGridViewColumn GridCol in dgvRegister.Columns)
                            {
                                if (GridCol.Visible == true)
                                {
                                    e.Graphics.FillRectangle(new SolidBrush(Color.LightGray),
                                        new System.Drawing.Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                                        (int)arrColumnWidths[iCount], iHeaderHeight));

                                    e.Graphics.DrawRectangle(Pens.Black,
                                        new System.Drawing.Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                                        (int)arrColumnWidths[iCount], iHeaderHeight));

                                    e.Graphics.DrawString(GridCol.HeaderText, GridCol.InheritedStyle.Font,
                                        new SolidBrush(GridCol.InheritedStyle.ForeColor),
                                        new RectangleF((int)arrColumnLefts[iCount], iTopMargin,
                                        (int)arrColumnWidths[iCount], iHeaderHeight), strFormat);
                                    iCount++;
                                }
                                
                            }
                            bNewPage = false;
                            iTopMargin += iHeaderHeight;
                        }
                        //Draw Columns Contents 
                        iCount = 0;
                        foreach (DataGridViewCell Cel in GridRow.Cells)
                        {
                            if (Cel.Visible != false)
                            {
                                if ((iRow + 1) % 2 == 0)
                                {
                                    e.Graphics.FillRectangle(new SolidBrush(dgvRegister.AlternatingRowsDefaultCellStyle.BackColor),
                                      new System.Drawing.Rectangle((int)arrColumnLefts[iCount], iTopMargin, (int)arrColumnWidths[iCount], iCellHeight));
                                }
                                if (Cel.Value != null)
                                {

                                    e.Graphics.DrawString(Cel.Value.ToString(), Cel.InheritedStyle.Font,
                                                new SolidBrush(Cel.InheritedStyle.ForeColor),
                                                new RectangleF((int)arrColumnLefts[iCount], (float)iTopMargin,
                                                (int)arrColumnWidths[iCount], (float)iCellHeight), strFormat);
                                }
                                //Drawing Cells Borders 
                                e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle((int)arrColumnLefts[iCount],
                                        iTopMargin, (int)arrColumnWidths[iCount], iCellHeight));
                                iCount++;
                            }
                            
                        }
                    }
                    iRow++;
                    iTopMargin += iCellHeight;
                }
                //If more lines exist, print another page.
                if (bMorePagesToPrint) e.HasMorePages = true;
                else e.HasMorePages = false;
            }
            catch (Exception exc) { MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            // dgvRegister = DGV;
        }
        //*        
        private void PDFRender()
        {
            if (File.Exists(DocNamex)) File.Delete(DocNamex);
            document = new PdfDocument(DocNamex);
            document.Info.Creator = "SosiDB software © 2015 AdureX Corp.";
            PdfPage page = document.AddPage();
            page.Size = PageSize.A4;
            XSize size = PageSizeConverter.ToSize(PdfSharp.PageSize.A4);
            if (OrienLandscape == true)
            {
                page.Width = size.Height; page.Height = size.Width;
                XMrgBdLeft = 40; XMrgBdTop = 60; XMrgBdWidth = 742; XMrgBdHeight = 495;
            }
            else
            {
                XMrgBdLeft = 60; XMrgBdTop = 40; XMrgBdWidth = 495; XMrgBdHeight = 742;
            }
            XGraphics gfx = XGraphics.FromPdfPage(page);

            try
            {
                XStringFormat xstrFormat = new XStringFormat();
                xstrFormat.Alignment = XStringAlignment.Near;
                xstrFormat.LineAlignment = XLineAlignment.Center;
                //xstrFormat.Trimming = StringTrimming.EllipsisCharacter; 
                arrColumnLefts.Clear();
                arrColumnWidths.Clear();
                iCellHeight = 0;
                iRow = 0;
                bFirstPage = true;
                bNewPage = true;

                // Calculating Total Widths
                iTotalWidth = 0;
                foreach (DataGridViewColumn dgvGridCol in dgvRegister.Columns)
                {
                    if (dgvGridCol.Visible == true) iTotalWidth += dgvGridCol.Width;
                }

                //Set the left margin
                int iLeftMargin = XMrgBdLeft;
                //Set the top margin
                int iTopMargin = XMrgBdTop;
                //Whether more pages have to print or not
                bool bMorePagesToPrint = false;
                int iTmpWidth = 0;

                //For the first page to print set the cell width and header height
                if (bFirstPage)
                {
                    foreach (DataGridViewColumn GridCol in dgvRegister.Columns)
                    {
                        if (GridCol.Visible == true)
                        {
                            iTmpWidth = (int)(Math.Floor((double)((double)GridCol.Width /
                                        (double)iTotalWidth * (double)iTotalWidth * ((double)XMrgBdWidth / (double)iTotalWidth))));

                            iHeaderHeight = dgvRegister.ColumnHeadersHeight;
                            // Save width and height of headers
                            arrColumnLefts.Add(iLeftMargin);
                            arrColumnWidths.Add(iTmpWidth);
                            iLeftMargin += iTmpWidth;
                        }
                    }
                }
                //Loop till all the grid rows not get printed
                while (iRow <= dgvRegister.Rows.Count - 1)
                {
                    DataGridViewRow GridRow = dgvRegister.Rows[iRow];
                    //Set the cell height
                    iCellHeight = GridRow.Height + 10;
                    int iCount = 0;
                    //Check whether the current page settings allow more rows to print
                    if (iTopMargin + iCellHeight >= XMrgBdHeight + XMrgBdTop)
                    {
                        bNewPage = true;
                        bFirstPage = false;
                        bMorePagesToPrint = true;
                        break;
                    }
                    else
                    {
                        if (bNewPage)
                        {
                            //Draw header
                            XFont dgvFont = new XFont(dgvRegister.Font.FontFamily.Name, 14, XFontStyle.BoldItalic);
                            gfx.DrawString(AdHeader, dgvFont, XBrushes.Black, XMrgBdLeft, XMrgBdTop - gfx.MeasureString(AdHeader, dgvFont).Height - 13);

                            //Draw Date
                            String strDate = "";// "Printed by SosiDB software: " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString();
                            gfx.DrawString(strDate, dgvFont, XBrushes.Black, XMrgBdLeft + (XMrgBdWidth - gfx.MeasureString(strDate, dgvFont).Width),
                                    XMrgBdTop - gfx.MeasureString(AdHeader, dgvFont).Height - 13);

                            //Draw Column headers and contents                 
                            iTopMargin = XMrgBdTop;
                            foreach (DataGridViewColumn GridCol in dgvRegister.Columns)
                            {
                                if (GridCol.Visible == true)
                                {
                                    gfx.DrawRectangle(XPens.LightGray, new XSolidBrush(XColor.FromName("LightGray")),
                                        new XRect((int)arrColumnLefts[iCount], iTopMargin, (int)arrColumnWidths[iCount], iHeaderHeight));

                                    gfx.DrawRectangle(XPens.Black, new XRect((int)arrColumnLefts[iCount], iTopMargin, (int)arrColumnWidths[iCount], iHeaderHeight));

                                    XTextFormatter tfx = new XTextFormatter(gfx);
                                    XFont GridColFont = new XFont(GridCol.InheritedStyle.Font.FontFamily.Name, GridCol.InheritedStyle.Font.Size + 5,
                                                                 (PdfSharp.Drawing.XFontStyle)GridCol.InheritedStyle.Font.Style);
                                    tfx.DrawString(GridCol.HeaderText, GridColFont, new XSolidBrush((XColor)GridCol.InheritedStyle.ForeColor),
                                        new XRect((int)arrColumnLefts[iCount] + 5, iTopMargin + 5, (int)arrColumnWidths[iCount], iHeaderHeight), XStringFormats.TopLeft);
                                    iCount++;
                                }
                                
                            }
                            bNewPage = false;
                            iTopMargin += iHeaderHeight;
                        }

                        //Draw Cell Contents
                        iCount = 0;
                        foreach (DataGridViewCell Cel in GridRow.Cells)
                        {
                            XFont CelFont = new XFont(Cel.InheritedStyle.Font.FontFamily.Name, Cel.InheritedStyle.Font.Size + 1,
                                                     (PdfSharp.Drawing.XFontStyle)Cel.InheritedStyle.Font.Style);
                            if (Cel.Visible != false)
                            {
                                if ((iRow + 1) % 2 == 0)
                                {
                                    gfx.DrawRectangle(XPens.LightGray, new XSolidBrush((XColor)(dgvRegister.AlternatingRowsDefaultCellStyle.BackColor)),
                                             new XRect((int)arrColumnLefts[iCount], iTopMargin, (int)arrColumnWidths[iCount], iCellHeight));
                                }
                                if (Cel.Value != null)
                                {
                                    XTextFormatter tf = new XTextFormatter(gfx);
                                    tf.DrawString(Cel.Value.ToString(), CelFont, new XSolidBrush((XColor)Cel.InheritedStyle.ForeColor),
                                                new XRect((int)arrColumnLefts[iCount] + 2, (float)iTopMargin + 3,
                                                (int)arrColumnWidths[iCount], iCellHeight), XStringFormats.TopLeft);
                                }
                                //Drawing Cells Borders  if(dgvRegister.AlternatingRowsDefaultCellStyle. 
                                gfx.DrawRectangle(XPens.Black, new XRect((int)arrColumnLefts[iCount],
                                        iTopMargin, (int)arrColumnWidths[iCount], iCellHeight));
                                iCount++;
                            }
                        }
                    }
                    iRow++;
                    iTopMargin += iCellHeight;
                }
                //If more lines exist, print another page.
                 //if (bMorePagesToPrint)  e.HasMorePages = true; else e.HasMorePages = false; 
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        // ******************* End *************************************************
    }
}