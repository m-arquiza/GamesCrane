﻿#pragma checksum "C:\Users\Miche\source\repos\m-arquiza\GamesCrane\GamesCrane\GamesCrane\View\AddPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "54E4CC1F1400F65C71CC09305CD59229815FE8037CE7EAB1102FB1EA0C6632AE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GamesCrane.View
{
    partial class AddPage : 
        global::Microsoft.UI.Xaml.Controls.Page, 
        global::Microsoft.UI.Xaml.Markup.IComponentConnector
    {

        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 1.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // View\AddPage.xaml line 118
                {
                    this.BackButton = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                }
                break;
            case 3: // View\AddPage.xaml line 121
                {
                    this.SendDetailsButton = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.SendDetailsButton).Click += this.VerifyAndSend;
                }
                break;
            case 4: // View\AddPage.xaml line 103
                {
                    this.SelectedImage = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Image>(target);
                }
                break;
            case 5: // View\AddPage.xaml line 106
                {
                    this.SelectImageButton = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.SelectImageButton).Click += this.SelectImage;
                }
                break;
            case 6: // View\AddPage.xaml line 111
                {
                    this.SelectedImagePath = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 7: // View\AddPage.xaml line 79
                {
                    global::Microsoft.UI.Xaml.Controls.CheckBox element7 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.CheckBox>(target);
                    ((global::Microsoft.UI.Xaml.Controls.CheckBox)element7).Checked += this.handleAdmin;
                    ((global::Microsoft.UI.Xaml.Controls.CheckBox)element7).Unchecked += this.handleAdmin;
                }
                break;
            case 8: // View\AddPage.xaml line 82
                {
                    global::Microsoft.UI.Xaml.Controls.CheckBox element8 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.CheckBox>(target);
                    ((global::Microsoft.UI.Xaml.Controls.CheckBox)element8).Checked += this.handleFlags;
                    ((global::Microsoft.UI.Xaml.Controls.CheckBox)element8).Unchecked += this.handleFlags;
                }
                break;
            case 9: // View\AddPage.xaml line 58
                {
                    this.PathBox = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBox>(target);
                    ((global::Microsoft.UI.Xaml.Controls.TextBox)this.PathBox).TextChanged += this.EnableAdd;
                }
                break;
            case 10: // View\AddPage.xaml line 37
                {
                    this.TitleBox = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBox>(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 1.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Microsoft.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Microsoft.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

