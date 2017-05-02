<%@ Page Title="LCD Message Sender" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LCDMessage._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<h1 >NOTE: Enclosure complete... come see!</h1>
                <h2>
                    Send a message to the LCD
                </h2>
    <asp:Table ID="Table1" runat="server">
        <asp:TableRow>
            <asp:TableCell>
                    <asp:TextBox ID="txtLCDMessage" runat="server" Rows="4" Columns="20" Font-Names="Courier New"
                        Height="60px" MaxLength="80" TextMode="MultiLine" Width="180px" Wrap="True" AutoPostBack="False"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell>
                    <asp:Button ID="btnSend" runat="server" Text="Send" OnClick="btnSend_Click" />
                    <br />
                    <asp:Button ID="btnPreview" runat="server" Text="Preview" OnClick="btnPreview_Click" />
                <p>
                    <asp:Label ID="lblStatus" runat="server" Text="Send a message" />
                </p>
            </asp:TableCell><asp:TableCell>
                <p>
                    What will be displayed:
                </p>
                <p><asp:TextBox Enabled="false" ID="txtMessagePreview" runat="server" ReadOnly="True" TextMode="MultiLine" Wrap="False" Columns="20" Rows="4"></asp:TextBox></p>
                <asp:Label ID="lblUser" runat="server" Visible="False"></asp:Label>
            </asp:TableCell></asp:TableRow></asp:Table><asp:Table ID="Table2" runat="server">
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Enclosure.jpg"
                    Width="533px"></asp:Image><br />
                <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/LCDMessageOnmycube.jpg"
                    Width="533px"></asp:Image>
            </asp:TableCell><asp:TableCell runat="server" ID="messageLogArea" VerticalAlign="Top"></asp:TableCell></asp:TableRow></asp:Table></asp:Content>