<%@ Page Title="Worker Summary" Language="C#" MasterPageFile="~/IncInc.Master" AutoEventWireup="true" CodeBehind="Summary.aspx.cs" Inherits="IncIncASP.Summary" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <br />
    <div class="col-md-4">
        <ul class="list-group list-group-flush">
            <li class="list-group-item">Total Workers Paid: <asp:Label ID="lblWorkerCountOutput" runat="server" /></li>
            <li class="list-group-item">Total Messages: <asp:Label ID="lblTotalMessagesOutput" runat="server" /></li>
            <li class="list-group-item">Overall Pay: <asp:Label ID="lblOverallPayOutput" runat="server" /></li>
            <li class="list-group-item">Average Pay: <asp:Label ID="lblOverallAverageOutput" runat="server" /></li>
        </ul><br />
    </div>
</asp:Content>
