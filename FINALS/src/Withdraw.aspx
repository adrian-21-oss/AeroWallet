<%@ Page Title="Withdraw" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Withdraw.aspx.cs" Inherits="FINALS.src.Withdraw"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .withdraw-header {
            font-size: 28px;
            font-weight: bold;
            color: #333;
            margin-top: 20px;
            display: block;
        }

        .page-header {
            margin-bottom: 2rem;
            color: #2c3e50;
            font-weight: 700;
            font-size:3rem;
        }

        .withdraw-container {
            padding-left: 30px;
            padding-top: 30px;
            height: 345px;
            max-width: 1320px;
            border-radius: 20px 14px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
            box-sizing: border-box; /* Ensures padding doesn't push width/height */

            border: 1px solid #cdecff;
            background: linear-gradient(135deg, #e0fcff 0%, #ffffff 100%);
        }

        .balance-display {
            font-size: 18px;
            font-weight: 600;
            color: #2c3e50;
            display: block;
            margin-bottom: 20px;
        }

        .input-label {
            font-size: 16px;
            color: #444;
            display: block;
            margin-bottom: 8px;
        }

        .modern-input {
            padding: 8px 12px;
            border: 1px solid #ccc;
            border-radius: 6px;
            width: 250px;
            font-size: 16px;
        }

        .modern-input:hover {
             background-color: #e0e0e0;
             border-color: #999;
             transition: all 0.2s;
        }

        .modern-button {
            padding: 10px 25px;
            background-color: #f0f0f0;
            border: 1px solid #bbb;
            border-radius: 6px;
            cursor: pointer;
            font-weight: bold;
            transition: all 0.2s;
            margin-top: 40px;
        }

        .modern-button:hover {
            background-color: #e0e0e0;
            border-color: #999;
        }

        .error-message {
            font-size: 13px;
            display: block;
            margin-top: 5px;
        }

        
        .transfer-disclaimer {
            
            font-size:12px;

            color: #718096;

            margin-top:50px;
            margin-bottom:-30px;
            max-width:320px;
        }

    </style>

    <main>
        <br />
        <br />

        <h1  class="page-header">WITHDRAW</h1>
        <hr style="border: 0; border-top: 1px solid #ccc; margin-bottom: 20px;" />
        
        <div class="withdraw-container">
            <asp:Label ID="yourBalance" runat="server" CssClass="balance-display"></asp:Label>
            
            <span class="input-label">Amount to withdraw:</span>
            <asp:TextBox id="withdrawalAmount" runat="server" CssClass="modern-input" placeholder="0.00"></asp:TextBox>
            
            <asp:RangeValidator ID="rv_withdrawalAmount" runat="server"
                ControlToValidate="withdrawalAmount"
                MaximumValue="2000"
                MinimumValue="100"
                Type="Integer"
                ErrorMessage="Must be minimum of 100 and maximum of 2000"
                Forecolor="Red"
                CssClass="error-text"
                Display="Dynamic">
            </asp:RangeValidator>
            
            <asp:Label ID="message" runat="server" ForeColor="Red" CssClass="error-message"></asp:Label>


            <p class="transfer-disclaimer">
                Please keep your generated withdrawal code and reference PIN strictly confidential. CloudMoney is not liable for cash claimed by third parties due to shared or compromised codes.
            </p>

            <div style="margin-top: 10px;">
                <asp:Button id="go_withdraw" text="Withdraw" OnClick="withdraw" runat="server" CssClass="modern-button"/>
            </div>
        </div>
        <br />
</asp:Content>