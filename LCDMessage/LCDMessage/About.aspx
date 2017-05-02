<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="About.aspx.cs" Inherits="LCDMessage.About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        About the LCD Message Sender
    </h2>
    <p>
        This is a site to send text to a 20x4 LCD screen sitting on the edge of Herrick's cube.
        <br />Messages can be formatted to fit a 20 column by 4 row screen.  Each send will clear the screen and set the new text on the screen.
        <br />The message is routed through this website, to a RF 2.4gz transmitter, to a similar transmitter sitting on an Arduino that will handle reading the serial data, formatting it, and sending it on to the screen. 
    </p>
</asp:Content>
