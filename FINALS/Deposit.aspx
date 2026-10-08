<%@ Page Title="Deposit" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Deposit.aspx.cs" Inherits="FINALS.Deposit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .deposit-header {
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

        .deposit-container {
     
            height: 300px;
            max-width: 1320px; 
            border-radius: 20px 14px;
            box-sizing: border-box;
            margin: 20px 0;
            padding: 30px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);

            border: 1px solid #cdecff;
            background: linear-gradient(135deg, #e0fcff 0%, #ffffff 100%);

        }

        .input-label {
            font-size: 16px;
            font-weight: 600;
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
            margin-top: 20px;
            margin-bottom:10px;
        }

        .modern-button:hover {
            background-color: #e0e0e0;
            border-color: #999;
        }

        .error-text {
            font-size: 13px;
            display: block;
            margin-top: 5px;
        }


        
        .transfer-disclaimer {
            
            font-size:12px;

            color: #718096;

            margin-top:40px;
            margin-bottom:10px;
            max-width:320px;
        }
    </style>

    <main>
        <br />
        <br />

        <h1 class="page-header">DEPOSIT</h1>
        <hr style="border: 0; border-top: 1px solid #ccc; margin-bottom: 20px;" />

        <div class="deposit-container">
            <span class="input-label">Amount to deposit:</span>
            
            <asp:TextBox id="depositAmount" runat="server" CssClass="modern-input" placeholder="100 - 2000"></asp:TextBox>
            
            <asp:RequiredFieldValidator ID="rfv_depositAmount" runat="server"
                ControlToValidate="depositAmount"
                ErrorMessage="Please input a value."
                Forecolor="Red"
                Display="Dynamic"
                >
            </asp:RequiredFieldValidator>

            <asp:RangeValidator ID="rv_depositAmount" runat="server"
                ControlToValidate="depositAmount"
                MaximumValue="2000"
                MinimumValue="100"
                Type="Integer"
                ErrorMessage="Must be minimum of 100 and maximum of 2000"
                Forecolor="Red"
                CssClass="error-text"
                Display="Dynamic">
            </asp:RangeValidator>

            <asp:Label ID="message" runat="server" ForeColor="Red" CssClass="error-text"></asp:Label>

            
            <p class="transfer-disclaimer">
                Funds will be credited to your account balance instantly after transaction approval. Please keep your reference receipt until the amount reflects.
            </p>

            <asp:Button id="go_deposit" text="Deposit" OnClick="deposit" runat="server" CssClass="modern-button"/>
        </div>
    </main>
</asp:Content>