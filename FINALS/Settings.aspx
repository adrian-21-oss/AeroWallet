<%@ Page Title="Settings" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="FINALS.Settings" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .settings-header {
            font-size: 28px;
            font-weight: bold;
            color: #333;
            margin-top: 20px;
            display: block;
        }
        .page-header {
            margin-bottom: 2rem;
            color: #2c3e50;
            font-size: 3rem;
            font-weight: 700;
        }

        .settings-container {
            padding: 30px;
            height: 500px;
            max-width: 1320px;
            border-radius: 20px 14px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);

            position: relative;
            box-sizing: border-box;

            border: 1px solid #cdecff;

            background: linear-gradient(135deg, #e0fcff 0%, #ffffff 100%);


        }


        
        .settings-container-2 {
            padding: 30px;

            max-width: 1320px;
            border-radius: 20px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);

            position: relative;
            box-sizing: border-box;

            border: 1px solid #cdecff;

            background: linear-gradient(135deg, #e0fcff 0%, #ffffff 100%);


        }


        .settings-container h2 {
            font-size: 20px;
            color: #2c3e50;
            margin-bottom: 25px;
        }

        .form-group {
            margin-bottom: 1.5rem;
            display: flex;
            align-items: center;
        }

        

        .input-label {
            width: 180px;     
            font-size: 14px;
            color: #2d3748;
            flex-shrink: 0;
        }

        .modern-input {
            padding: 6px 10px;
            border: 1px solid #ccc;
            border-radius: 4px;
            width: 250px;      
        }

        .modern-input:hover {
             background-color: #e0e0e0;
             border-color: #999;
             transition: all 0.2s;

        }

        .validator-text {
            font-size: 12px;
            margin-left: 10px;
        }

        .btn-change {
            padding: 10px 20px;
            background-color: #f0f0f0;
            border: 1px solid #bbb;
            border-radius: 6px;
            cursor: pointer;
            font-weight: bold;
            margin-top: 20px;
            transition: all 0.2s;
        }

        .btn-change:hover {
            background-color: #e0e0e0;
        }

        .btn-logout {
            padding: 6px 15px;
            background-color: #fff0f0;
            border: 1px solid #ffcccc;
            color: #cc0000;
            border-radius: 4px;
            cursor: pointer;
            width: 100%;
            font-weight: 600;
        }

        .btn-logout:hover {
            background-color: #fee2e2;
            transition: all 0.2s;
        }
    </style>

    <main>
        <br />
        <br />

        <h1 class="page-header">SETTINGS</h1>
        <hr style="border: 0; border-top: 1px solid #ccc; margin-bottom: 20px;" />
        
        <div class="settings-container">
            <p style="font-size: 28px ; font-weight:700;">Change Password</p>
            
            <br />
            <br />




            <div class="form-group">
                <!-- <span class="input-label">Current Password:</span> -->
                <asp:TextBox ID="currentPassword_Input" placeholder="Current Password" TextMode="Password" runat="server" CssClass="modern-input"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_currentPassword" runat="server"
                    ControlToValidate="currentPassword_Input" ErrorMessage="Required"
                    Forecolor="Red" CssClass="validator-text" Display="Dynamic">
                </asp:RequiredFieldValidator>
            </div>



            <div class="form-group">
                <!--<span class="input-label">New Password:</span> -->
                <asp:TextBox ID="newPassword_Input" placeholder="New Password" TextMode="Password" runat="server" CssClass="modern-input"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_newPassword" runat="server"
                    ControlToValidate="newPassword_Input" ErrorMessage="Required"
                    Forecolor="Red" CssClass="validator-text" Display="Dynamic">
                </asp:RequiredFieldValidator>
            </div>


            <div class="form-group">
                <!-- <span class="input-label">Confirm New Password:</span> -->
                <asp:TextBox ID="confirmNewPassword_Input" placeholder="Confirm New Password"  TextMode="Password" runat="server" CssClass="modern-input"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_confirmNewPassword" runat="server"
                    ControlToValidate="confirmNewPassword_Input" ErrorMessage="Required"
                    Forecolor="Red" CssClass="validator-text" Display="Dynamic">
                </asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cv_confirmNewPassword" runat="server"
                    ControlToCompare="newPassword_Input" ControlToValidate="confirmNewPassword_Input"
                    ErrorMessage="Passwords do not match" Forecolor="Red" 
                    CssClass="validator-text" Display="Dynamic">
                </asp:CompareValidator>

                <br />

            </div>

            <br />

            <asp:Button id="changePass" OnClick="Change_Password" text="Change Password" runat="server" CssClass="btn-change"/>
            <br />
            <asp:Label ID="message" runat="server" ForeColor="Red" CssClass="validator-text" style="margin-top:10px; display:block;"></asp:Label>

        </div>

        <br />
        <br />

        <div class="settings-container-2">
            <p style="font-size: 28px ; font-weight:700;">Account Session</p>
            
            <div style="margin-left: auto; margin-right:20px; margin-top:15px; margin-bottom:10px; width: 98px;">
                <asp:Button id="logOut" CausesValidation="false" OnClick="Log_Out" text="Log Out" runat="server" CssClass="btn-logout"/>
            </div>
        </div>
        <br />



    </main>
</asp:Content>