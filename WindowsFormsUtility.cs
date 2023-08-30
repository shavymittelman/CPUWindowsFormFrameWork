using System.Data;

namespace CPUWindowsFormFramework
{
    public class WindowsFormsUtility
    {
        public static void SetListBinding(ComboBox lst, DataTable sourcedt, DataTable? targetdt, string tablename)
        {
            lst.DataSource = sourcedt;
            lst.ValueMember = tablename + "Id";
            lst.DisplayMember = lst.Name.Substring(3);
            if (targetdt != null)
            {
                lst.DataBindings.Add("SelectedValue", targetdt, lst.ValueMember, false, DataSourceUpdateMode.OnPropertyChanged);
            }
        }


        public static void SetControlBinding(Control ctrl, BindingSource bs)
        { 
            string propertyname = "";
            string controlname = ctrl.Name.ToLower();
            string controltype = controlname.Substring(0, 3);
            string columname = controlname.Substring(3);
             

            switch (controltype)
            {
                case "txt":
                case "lbl":
                    propertyname = "Text";
                    break;
                case "dtp":
                    
                    propertyname = "Value";
                    break;
                case "chk":
                    propertyname = "Checked";
                    break;
            }

            if (propertyname != "" && columname != "")
            {
                ctrl.DataBindings.Add(propertyname, bs, columname, true, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        public static void FormatGridForSearchResults(DataGridView grid, string tablename)
        {
            grid.AllowUserToAddRows = false;
            grid.ReadOnly = true;            
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DoFormatgrid(grid, tablename);
        }

        public static void FormatGridForEdit(DataGridView grid, string tablename)
        {
            grid.EditMode = DataGridViewEditMode.EditOnEnter;
            DoFormatgrid(grid, tablename);
        }

        private static void DoFormatgrid(DataGridView grid, string tablename)
        {
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            grid.RowHeadersWidth = 25;
            foreach (DataGridViewColumn col in grid.Columns)
            {
                if (col.Name.EndsWith("Id"))
                {
                    col.Visible = false;
                }
            }
            string pkname = tablename + "id";
            if (grid.Columns.Contains(pkname))
            {
                grid.Columns[pkname].Visible = false;
            }
        }

        public static int GetIdFromGrid(DataGridView grid, int rowindex, string columnname)
        {
            int id = 0;
            if (rowindex < grid.Rows.Count && grid.Columns.Contains(columnname) && grid.Rows[rowindex].Cells[columnname].Value != DBNull.Value)
            {
                if (grid.Rows[rowindex].Cells[columnname].Value is int)
                {
                    id = (int)grid.Rows[rowindex].Cells[columnname].Value;
                }
            }
                      
            return id;
        }

        public static int GetIdFromComboBox(ComboBox lst)
        {
            int value = 0;
            if (lst.SelectedValue != null && lst.SelectedValue is int)
            {
                value = (int)lst.SelectedValue;
            }
            return value;
        }

        public static void AddDeleteButtonToGrid(DataGridView grid, string deletecolname)
        {
            grid.Columns.Add(new DataGridViewButtonColumn() { Text = "X", HeaderText = "Delete", Name = deletecolname, UseColumnTextForButtonValue = true});
            grid.Columns[deletecolname].DisplayIndex = grid.ColumnCount - 1;
        }

        public static void AddComboBoxToGrid(DataGridView grid, DataTable datasource, string tablename, string displaymember)
        {
            DataGridViewComboBoxColumn c = new();
            c.DataSource = datasource;
            c.DisplayMember = displaymember;
            c.ValueMember = tablename + "id";
            c.DataPropertyName = c.ValueMember;
            c.HeaderText = tablename;
            grid.Columns.Insert(0, c);
        }

        public static bool IsFormOpen(Type formtype, int pkvalue)
        {
            bool exists = false;
            foreach (Form frm in Application.OpenForms)
            {
                int frmpkvalue = 0;
                if (frm.Tag != null && frm.Tag is int)
                {
                    frmpkvalue = (int)frm.Tag;
                }
                if (frm.GetType() == formtype && frmpkvalue == pkvalue)
                {
                    frm.Activate();
                    exists = true;
                    break;
                }
            }
            return exists;
        }

        public static void SetupNav(ToolStrip ts)
        {
            ts.Items.Clear();
            foreach (Form f in Application.OpenForms)
            {
                if (f.IsMdiContainer == false)
                {
                    ToolStripButton btn = new ToolStripButton();
                    btn.Text = f.Text;
                    btn.Tag = f;
                    btn.Click += Btn_Click;
                    ts.Items.Add(btn);
                    ts.Items.Add(new ToolStripSeparator());
                }
            }
        }

        public static void HideColumn(DataGridView grid, string columnname)
        {
            grid.Columns[columnname].Visible = false;
        }

        public static void DisableAndEnableButtons(List<Button> lstbuttons, bool b = false)
        {
            lstbuttons.ForEach(b => b.Enabled = false);
            if (b == true)
            {
                lstbuttons.ForEach(b => b.Enabled = true);
            }
        }

        private static  void Btn_Click(object? sender, EventArgs e)
        {
            if (sender != null && sender is ToolStripButton)
            {
                ToolStripButton btn = (ToolStripButton)sender;
                if (btn.Tag != null && btn.Tag is Form)
                {
                    ((Form)btn.Tag).Activate();
                    
                }
            }
        }
    }
}
