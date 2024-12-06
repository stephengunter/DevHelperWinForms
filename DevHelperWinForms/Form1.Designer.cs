namespace DevHelperWinForms
{
   partial class Form1
   {
      private System.ComponentModel.IContainer components = null;

      // ComboBoxes for each row
      private TextBox txt_root;
      private TextBox txt_core;
      private TextBox txt_web;
      private TextBox txt_template;
      private TextBox txt_name;

      // Labels for each row
      private Label lbl_root;
      private Label lbl_corePath;
      private Label lbl_web;
      private Label lbl_template;
      private Label lbl_name;


      private Button btn_submit;

      private FlowLayoutPanel chatPanel;
      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         txt_root = new TextBox();
         txt_core = new TextBox();
         txt_web = new TextBox();
         txt_template = new TextBox();
         txt_name = new TextBox();
         lbl_root = new Label();
         lbl_corePath = new Label();
         lbl_web = new Label();
         lbl_template = new Label();
         lbl_name = new Label();
         btn_submit = new Button();
         SuspendLayout();
         // 
         // txt_root
         // 
         txt_root.Location = new Point(105, 15);
         txt_root.Margin = new Padding(3, 2, 3, 2);
         txt_root.Name = "txt_root";
         txt_root.Size = new Size(307, 23);
         txt_root.TabIndex = 1;
         // 
         // txt_core
         // 
         txt_core.Location = new Point(105, 45);
         txt_core.Margin = new Padding(3, 2, 3, 2);
         txt_core.Name = "txt_core";
         txt_core.Size = new Size(307, 23);
         txt_core.TabIndex = 3;
         // 
         // txt_web
         // 
         txt_web.Location = new Point(105, 78);
         txt_web.Margin = new Padding(3, 2, 3, 2);
         txt_web.Name = "txt_web";
         txt_web.Size = new Size(307, 23);
         txt_web.TabIndex = 5;
         // 
         // txt_template
         // 
         txt_template.Location = new Point(105, 105);
         txt_template.Margin = new Padding(3, 2, 3, 2);
         txt_template.Name = "txt_template";
         txt_template.Size = new Size(520, 23);
         txt_template.TabIndex = 7;
         // 
         // txt_name
         // 
         txt_name.Location = new Point(105, 135);
         txt_name.Margin = new Padding(3, 2, 3, 2);
         txt_name.Name = "txt_name";
         txt_name.Size = new Size(307, 23);
         txt_name.TabIndex = 9;
         // 
         // lbl_root
         // 
         lbl_root.Location = new Point(18, 15);
         lbl_root.Name = "lbl_root";
         lbl_root.Size = new Size(70, 19);
         lbl_root.TabIndex = 0;
         lbl_root.Text = "Root:";
         // 
         // lbl_corePath
         // 
         lbl_corePath.Location = new Point(18, 45);
         lbl_corePath.Name = "lbl_corePath";
         lbl_corePath.Size = new Size(75, 20);
         lbl_corePath.TabIndex = 2;
         lbl_corePath.Text = "Core:";
         // 
         // lbl_web
         // 
         lbl_web.Location = new Point(18, 75);
         lbl_web.Name = "lbl_web";
         lbl_web.Size = new Size(75, 20);
         lbl_web.TabIndex = 4;
         lbl_web.Text = "Web:";
         // 
         // lbl_template
         // 
         lbl_template.Location = new Point(18, 105);
         lbl_template.Name = "lbl_template";
         lbl_template.Size = new Size(75, 20);
         lbl_template.TabIndex = 6;
         lbl_template.Text = "Template:";
         // 
         // lbl_name
         // 
         lbl_name.Location = new Point(18, 135);
         lbl_name.Name = "lbl_name";
         lbl_name.Size = new Size(75, 20);
         lbl_name.TabIndex = 8;
         lbl_name.Text = "Name:";
         // 
         // btn_submit
         // 
         btn_submit.Location = new Point(337, 175);
         btn_submit.Name = "btn_submit";
         btn_submit.Size = new Size(75, 23);
         btn_submit.TabIndex = 10;
         btn_submit.Text = "確定";
         btn_submit.UseVisualStyleBackColor = true;
         btn_submit.Click += btn_submit_Click;
         // 
         // chatPanel
         // 
         chatPanel = new FlowLayoutPanel
         {
            Name = "chatPanel",
            Size = new Size(400, 200), // Width x Height
            Location = new Point(20, 215), // X, Y coordinates
            AutoScroll = true,
            BorderStyle = BorderStyle.FixedSingle,
         };



         // 
         // Form1
         // 
         AutoScaleDimensions = new SizeF(7F, 15F);
         AutoScaleMode = AutoScaleMode.Font;
         ClientSize = new Size(700, 480);
         Controls.Add(btn_submit);
         Controls.Add(lbl_root);
         Controls.Add(txt_root);
         Controls.Add(lbl_corePath);
         Controls.Add(txt_core);
         Controls.Add(lbl_web);
         Controls.Add(txt_web);
         Controls.Add(lbl_template);
         Controls.Add(txt_template);
         Controls.Add(lbl_name);
         Controls.Add(txt_name);
         Controls.Add(chatPanel);

         Margin = new Padding(3, 2, 3, 2);
         Name = "Form1";
         Text = "Add Models";
         Load += Form1_Load;
         ResumeLayout(false);
         PerformLayout();
      }
   }
}
