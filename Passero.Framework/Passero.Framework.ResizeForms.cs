﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;

using Wisej.Web;

namespace Passero.Framework
{



    public class ControlStorage
    {


        public List<System.Drawing.Rectangle> bounds = new List<System.Drawing.Rectangle>();
        // questa funziona solo su Windows
        public List<System.Drawing.Font> font = new List<System.Drawing.Font>();
        //
        public List<object> datagridview_defaultcellstyle = new List<object>();
        public List<int> datagridview_defaultrowheight = new List<int>();
        public List<int> datagridview_columnheadersheight = new List<int>();
        public List<object> datagridview_columncollection = new List<object>();


    }

    public class FormResize
    {
        public System.Drawing.Font FormFont;
        public ControlStorage ControlStorage = new ControlStorage();
        // Private _form_font As System.Drawing.Font
        // Private showRowHeader As Boolean = False
        public FormResize(Form CallingForm)
        {
            Form = CallingForm; // the calling form
            _formSize = CallingForm.ClientSize; // Save initial form size
            _fontsize = CallingForm.Font.Size; // Font size
        }


        private float private_fontsize;
        private float _fontsize
        {
            get
            {
                return private_fontsize;
            }
            set
            {
                private_fontsize = value;
            }
        }

        private System.Drawing.SizeF private_formSize;
        private System.Drawing.SizeF _formSize
        {
            get
            {
                return private_formSize;
            }
            set
            {
                private_formSize = value;
            }
        }

        private Form privateform;
        private Form Form
        {
            get
            {
                return privateform;
            }
            set
            {
                privateform = value;
            }
        }

        public void GetInitialSize() // get initial size//
        {

            ControlStorage.bounds.Clear();
            ControlStorage.font.Clear();
            ControlStorage.datagridview_columncollection.Clear();
            ControlStorage.datagridview_defaultcellstyle.Clear();
            ControlStorage.datagridview_defaultrowheight.Clear();
            ControlStorage.datagridview_columnheadersheight.Clear();


            FormFont = Form.Font;

            var _controls = GetAllFormControls(Form); // call the enumerator
            foreach (Control control in _controls) // Loop through the controls
            {
                ControlStorage.bounds.Add(control.Bounds); // saves control bounds/dimension
                ControlStorage.font.Add(control.Font);
                // If you have datagridview
                if (ReferenceEquals(control.GetType(), typeof(DataGridView)))
                {
                    Wisej.Web .DataGridView _dgv = (Wisej.Web .DataGridView)control;
                    DataGridViewColumnCollection _nColumns = new Wisej .Web. DataGridViewColumnCollection(_dgv);
                    foreach (Wisej.Web.DataGridViewColumn _col in _dgv.Columns)
                    {
                        _nColumns.Add((Wisej.Web.DataGridViewColumn)_col.Clone());
                    }
                    ControlStorage.datagridview_defaultcellstyle.Add(_dgv.DefaultCellStyle.Clone());
                    ControlStorage.datagridview_columncollection.Add(_nColumns);
                    ControlStorage.datagridview_defaultrowheight.Add(_dgv.DefaultRowHeight);
                    ControlStorage.datagridview_columnheadersheight.Add(_dgv.ColumnHeadersHeight);
                }
                // _dgv_Column_Adjust((CType(control, DataGridView)), showRowHeader)
                else
                {
                    ControlStorage.datagridview_defaultcellstyle.Add("");
                    ControlStorage.datagridview_columncollection.Add("");
                    ControlStorage.datagridview_defaultrowheight.Add(0);
                    ControlStorage.datagridview_columnheadersheight.Add(0);
                }
            }
        }

        public void Resize(ControlStorage CS) // Set the resize
        {
            double _form_ratio_width = Form.ClientSize.Width / (double)_formSize.Width; // ratio could be greater or less than 1
            double _form_ratio_height = Form.ClientSize.Height / (double)_formSize.Height; // this one too
            var _controls = GetAllFormControls(Form); // reenumerate the control collection
            int _pos = -1; // do not change this value unless you know what you are doing


            foreach (Control control in _controls)
            {

                // do some math calc
                _pos += 1; // increment by 1;
                var _controlSize = new System.Drawing.Size((int)Math.Round(Conversion.Fix(CS.bounds[_pos].Width * _form_ratio_width)), (int)Math.Round(Conversion.Fix(CS.bounds[_pos].Height * _form_ratio_height))); // use for sizing
                var _controlposition = new System.Drawing.Point((int)Math.Round(Conversion.Fix(CS.bounds[_pos].X * _form_ratio_width)), (int)Math.Round(Conversion.Fix(CS.bounds[_pos].Y * _form_ratio_height))); // use for location
                                                                                                                                                                                                                  // set bounds
                control.Bounds = new System.Drawing.Rectangle(_controlposition, _controlSize); // Put together
                                                                                               // set font
                /* TODO ERROR: Skipped WarningDirectiveTrivia
                #Disable Warning BC42016 ' Conversione implicita
                */
                float FontSize = (float)Conversion.Fix((double)CS.font[_pos].Size * _form_ratio_height);
                /* TODO ERROR: Skipped WarningDirectiveTrivia
                #Enable Warning BC42016 ' Conversione implicita
                */
                control.Font = new System.Drawing.Font(CS.font[_pos].FontFamily, FontSize, CS.font[_pos].Style);



                if (ReferenceEquals(control.GetType(), typeof(DataGridView)))
                {
                    DataGridView _dgv = (DataGridView)control;
                    DataGridViewColumnCollection _nColumns = (DataGridViewColumnCollection)CS.datagridview_columncollection[_pos];
                    _dgv.ColumnHeadersHeight = (int)Math.Round(Conversion.Fix(CS.datagridview_columnheadersheight[_pos] * _form_ratio_height));
                    _dgv.DefaultRowHeight = (int)Math.Round(Conversion.Fix(CS.datagridview_defaultrowheight[_pos] * _form_ratio_height));
                    DataGridViewCellStyle _dgvDefaultCellStyle = (DataGridViewCellStyle)CS.datagridview_defaultcellstyle[_pos];
                    if (_dgvDefaultCellStyle.Font is not null)
                    {
                        float _dgvDefaultCellStyleFontSize = (float)Conversion.Fix((double)_dgvDefaultCellStyle.Font.Size * _form_ratio_height);
                        _dgv.DefaultCellStyle.Font = new System.Drawing.Font(_dgvDefaultCellStyle.Font.FontFamily, _dgvDefaultCellStyleFontSize, _dgvDefaultCellStyle.Font.Style);
                    }

                    foreach (DataGridViewColumn _column in _dgv.Columns)
                    {
                        var _nColumn = _nColumns[_column.Name];
                        if (_nColumn.DefaultCellStyle.Font is not null)
                        {
                            float columnFontSize = (float)Conversion.Fix((double)_nColumn.DefaultCellStyle.Font.Size * _form_ratio_height);
                            _column.DefaultCellStyle.Font = new System.Drawing.Font(_nColumn.DefaultCellStyle.Font.FontFamily, columnFontSize, _nColumn.DefaultCellStyle.Font.Style);

                        }
                        if (_nColumn.HeaderStyle.Font is not null)
                        {
                            float headerFontSize = (float)Conversion.Fix((double)_nColumn.HeaderStyle.Font.Size * _form_ratio_height);
                            _column.HeaderStyle.Font = new System.Drawing.Font(_nColumn.HeaderStyle.Font.FontFamily, headerFontSize, _nColumn.HeaderStyle.Font.Style);
                        }

                        if (_nColumn is not null)
                        {
                            switch (_nColumn.AutoSizeMode)
                            {
                                case var @case when @case == (DataGridViewAutoSizeColumnMode.NotSet | DataGridViewAutoSizeColumnMode.None):
                                    {
                                        _column.Width = (int)Math.Round(Conversion.Fix(_nColumn.Width * _form_ratio_width));
                                        break;
                                    }

                                default:
                                    {
                                        break;
                                    }
                            }
                        }
                    }

                }


            }
        }

        private static IEnumerable<Control> GetAllFormControls(Control c)
        {
            return c.Controls.Cast<Control>().SelectMany(item => GetAllFormControls(item)).Concat(c.Controls.Cast<Control>()).Where(control => !string.IsNullOrEmpty(control.Name));
        }
    }

    public class PageResize
    {
        public System.Drawing.Font PageFont;
        public ControlStorage ControlStorage = new ControlStorage();
        // Private _form_font As System.Drawing.Font
        // Private showRowHeader As Boolean = False
        public PageResize(Page CallingPage)
        {
            Page = CallingPage; // the calling form
            _pageSize = CallingPage.ClientSize; // Save initial form size
            _fontsize = CallingPage.Font.Size; // Font size
        }


        private float private_fontsize;
        private float _fontsize
        {
            get
            {
                return private_fontsize;
            }
            set
            {
                private_fontsize = value;
            }
        }

        private System.Drawing.SizeF private_formSize;
        private System.Drawing.SizeF _pageSize
        {
            get
            {
                return private_formSize;
            }
            set
            {
                private_formSize = value;
            }
        }

        private Page privatepage;
        private Page Page
        {
            get
            {
                return privatepage;
            }
            set
            {
                privatepage = value;
            }
        }

        public void GetInitialSize(Control Container = null) // get initial size//
        {

            ControlStorage.bounds.Clear();
            ControlStorage.font.Clear();
            ControlStorage.datagridview_columncollection.Clear();
            ControlStorage.datagridview_defaultcellstyle.Clear();
            ControlStorage.datagridview_defaultrowheight.Clear();
            ControlStorage.datagridview_columnheadersheight.Clear();


            PageFont = Page.Font;


            var _controls = GetAllPageControls(privatepage); // call the enumerator
            foreach (Control control in _controls) // Loop through the controls
            {
                ControlStorage.bounds.Add(control.Bounds); // saves control bounds/dimension
                ControlStorage.font.Add(control.Font);
                // If you have datagridview
                if (ReferenceEquals(control.GetType(), typeof(DataGridView)))
                {
                    DataGridView _dgv = (DataGridView)control;
                  
                    var _nColumns = new DataGridViewColumnCollection(_dgv);
                  
                    foreach (DataGridViewColumn _col in _dgv.Columns)
                        _nColumns.Add((Wisej.Web.DataGridViewColumn)_col.Clone  ());

                    ControlStorage.datagridview_defaultcellstyle.Add(_dgv.DefaultCellStyle.Clone());
                    ControlStorage.datagridview_columncollection.Add(_nColumns);
                    ControlStorage.datagridview_defaultrowheight.Add(_dgv.DefaultRowHeight);
                    ControlStorage.datagridview_columnheadersheight.Add(_dgv.ColumnHeadersHeight);
                }
                // _dgv_Column_Adjust((CType(control, DataGridView)), showRowHeader)
                else
                {
                    ControlStorage.datagridview_defaultcellstyle.Add("");
                    ControlStorage.datagridview_columncollection.Add("");
                    ControlStorage.datagridview_defaultrowheight.Add(0);
                    ControlStorage.datagridview_columnheadersheight.Add(0);
                }
            }
        }

        public void Resize(ControlStorage CS) // Set the resize
        {
            double _page_ratio_width = Page.ClientSize.Width / (double)_pageSize.Width; // ratio could be greater or less than 1
            double _page_ratio_height = Page.ClientSize.Height / (double)_pageSize.Height; // this one too
            var _controls = GetAllPageControls(Page); // reenumerate the control collection
            int _pos = -1; // do not change this value unless you know what you are doing


            foreach (Control control in _controls)
            {

                // do some math calc
                _pos += 1; // increment by 1;
                var _controlSize = new System.Drawing.Size((int)Math.Round(Conversion.Fix(CS.bounds[_pos].Width * _page_ratio_width)), (int)Math.Round(Conversion.Fix(CS.bounds[_pos].Height * _page_ratio_height))); // use for sizing
                var _controlposition = new System.Drawing.Point((int)Math.Round(Conversion.Fix(CS.bounds[_pos].X * _page_ratio_width)), (int)Math.Round(Conversion.Fix(CS.bounds[_pos].Y * _page_ratio_height))); // use for location
                                                                                                                                                                                                                  // set bounds
                control.Bounds = new System.Drawing.Rectangle(_controlposition, _controlSize); // Put together
                                                                                               // set font
           
                float FontSize = (float)Conversion.Fix((double)CS.font[_pos].Size * _page_ratio_height);
                control.Font = new System.Drawing.Font(CS.font[_pos].FontFamily, FontSize, CS.font[_pos].Style);

                if (ReferenceEquals(control.GetType(), typeof(DataGridView)))
                {
                    DataGridView _dgv = (DataGridView)control;
                    DataGridViewColumnCollection _nColumns = (DataGridViewColumnCollection)CS.datagridview_columncollection[_pos];
                    _dgv.ColumnHeadersHeight = (int)Math.Round(Conversion.Fix(CS.datagridview_columnheadersheight[_pos] * _page_ratio_height));
                    _dgv.DefaultRowHeight = (int)Math.Round(Conversion.Fix(CS.datagridview_defaultrowheight[_pos] * _page_ratio_height));
                    DataGridViewCellStyle _dgvDefaultCellStyle = (DataGridViewCellStyle)CS.datagridview_defaultcellstyle[_pos];
                    if (_dgvDefaultCellStyle.Font is not null)
                    {
                        float _dgvDefaultCellStyleFontSize = (float)Conversion.Fix((double)_dgvDefaultCellStyle.Font.Size * _page_ratio_height);
                        _dgv.DefaultCellStyle.Font = new System.Drawing.Font(_dgvDefaultCellStyle.Font.FontFamily, _dgvDefaultCellStyleFontSize, _dgvDefaultCellStyle.Font.Style);
                    }

                    foreach (DataGridViewColumn _column in _dgv.Columns)
                    {
                        var _nColumn = _nColumns[_column.Name];
                        if (_nColumn.DefaultCellStyle.Font is not null)
                        {
                            float columnFontSize = (float)Conversion.Fix((double)_nColumn.DefaultCellStyle.Font.Size * _page_ratio_height);
                            _column.DefaultCellStyle.Font = new System.Drawing.Font(_nColumn.DefaultCellStyle.Font.FontFamily, columnFontSize, _nColumn.DefaultCellStyle.Font.Style);

                        }
                        if (_nColumn.HeaderStyle.Font is not null)
                        {
                            float headerFontSize = (float)Conversion.Fix((double)_nColumn.HeaderStyle.Font.Size * _page_ratio_height);
                            _column.HeaderStyle.Font = new System.Drawing.Font(_nColumn.HeaderStyle.Font.FontFamily, headerFontSize, _nColumn.HeaderStyle.Font.Style);
                        }

                        if (_nColumn is not null)
                        {
                            switch (_nColumn.AutoSizeMode)
                            {
                                case var @case when @case == (DataGridViewAutoSizeColumnMode.NotSet | DataGridViewAutoSizeColumnMode.None):
                                    {
                                        _column.Width = (int)Math.Round(Conversion.Fix(_nColumn.Width * _page_ratio_width));
                                        break;
                                    }

                                default:
                                    {
                                        break;
                                    }

                            }

                        }


                        // _dgv_Column_Adjust((CType(control, DataGridView)), showRowHeader)
                    }

                }


            }
        }


        private static IEnumerable<Control> GetAllPageControls(Control c)
        {
            return c.Controls.Cast<Control>().SelectMany(item => GetAllPageControls(item)).Concat(c.Controls.Cast<Control>()).Where(control => !string.IsNullOrEmpty(control.Name));
        }
    }
}