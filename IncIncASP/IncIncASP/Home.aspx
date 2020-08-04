<%@ Page Title="Index" Language="C#" MasterPageFile="~/IncInc.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="IncIncASP.Home" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <br />
    <asp:LoginView ID="loginviewBase" runat="server">  
        <AnonymousTemplate>  
            <p>Please either <a href="/Account/Login.aspx">Log In</a> or <a href="/Account/Register.aspx">Register</a> to continue.</p>
        </AnonymousTemplate>  
        <LoggedInTemplate>
            <p>You are currently logged in.</p>
            <asp:Button ID="btnPayroll" Text="Payroll Entry" OnClick="btnPayroll_Click" runat="server" />
            <asp:Button ID="btnSummary" Text="Payroll Summary" OnClick="btnSummary_Click" runat="server" />
            <asp:Button ID="btnTable" Text="Employee List" OnClick="btnTable_Click" runat="server" />
        </LoggedInTemplate>  
    </asp:LoginView>  
</asp:Content>
