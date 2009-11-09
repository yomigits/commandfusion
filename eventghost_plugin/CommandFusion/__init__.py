# EventGhost receiver plugin for CommandFusion Viewer solutions, such as iPhone/iPod Touch native application "CF iViewer"
# Copyright (C) 2009 CommandFusion <support@commandfusion.com>
# http://www.commandfusion.com
#  
# Based on the Network Receiver plugin.

eg.RegisterPlugin(
    name = "CommandFusion Plugin",
    description=(
        'Plugin for CommandFusion Viewer solutions, such as the CF iViewer app for iPhone/Ipod Touch<SMALL><SUP>TM</SUP></SMALL>.'
        '\n\n<p>'
        '<center><img src="cf_logo.png" /></a></center>'
    ),
    version = "1.0."+ "$LastChangedRevision: 1 $".split()[1],
	kind = "remote",
    canMultiLoad = True,
    author = "CommandFusion",
	url="http://www.commandfusion.com",
	help = """
        <p>Grab the latest copy of this plugin from http://commandfusion.googlecode.com</p>

        <p>1. Define the port to listen on.<br />
        2. Define a password (leaving password blank is also fine).<br />
        3. Setup your GUI in CommandFusion guiDesigner, set the project properties to the IP address of the machine running EventGhost, and the port number chosen earlier.<br />
        4. Define some buttons/gauges/sliders/text/images, and assign join numbers to them. These join numbers will be used to identify button presses, etc, within EventGhost.<br />
        5. Now once you have uploaded the GUI file to your iPhone or iPod Touch, when you press a button in the GUI you should see a message logged in EventGhost.<br />
        6. You can use these messages as event triggers to make certain things happen when a button is pressed or released, when a slider is dragged, etc.<br /></p>
    """,
    icon = (
		"iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJ"
        "bWFnZVJlYWR5ccllPAAAA51JREFUeNpck31oG2Ucx793ebm8NMml9uWyFntpzGi36jIzaWBOziJa"
        "ZKwoU6uy0b38IyjNRFAHcxZl/0lbKoLgWOofq2PMroiyyUZSC1bWDk9xgkt7vW5L2qxJe5dempfL"
        "i3cFEfaD3/PP8/w+z/f5/b4PgeBrF0DaBlDvhRa8lqO4/mkEj8Qbb55gCYIYbGnxcP4n2gN+vw8X"
        "Jy6PEBjja/j9ZgSxqwG0BgOw0gBBxOBpGRo6wqHN48b349/2WZf/Cuug5uZG+Nq9sRpq4qIgDBix"
        "mgCYDhbCzF6Utobh6QrD08E1Nrm574RN7Kg6wb71DrLrGZSiU2L2/p1jv87e4i0WKtrSysAAf88A"
        "lM0AOl+c/mA/E6pm7gUeFirY9OxE2vEYREcDFmwUVmx1WGWepOeLDuxO/93b1NzQazQaeANau1nk"
        "lFCoycB1sw6O1ai2bBKktQ7pRi8ItxuU04QyCWTup+Gb+jIQ9DEBijLrL/qYRKUyimoVR7tbWLvd"
        "Dp/Pi8N9vWh7eEcc20fzji0FSqoIIqPiQyzh/Vf2w2w26cWxc599EjEgHpU6nuHoI1xnqFwug2lu"
        "Qlvb4yDUAn3opRc6nSaLJSOVQp97Sf4gAya+KEBVVWhnj838EhVJHXX59OGpfL4Aq9UCl8sFC2WB"
        "sCiO+ByEdKbLdOrg7Ji7aek3OpFIwOlwgHY5sXfPU9vj3QbIsoxNRUGtBi1reJBIiu5699B/HtD2"
        "z3o8zazZTG3Ld2gQt9v1P+DHn65hY30DudwWShUS5+eKoHr6N0b+qQ2/PVujOzt2himK0mVrLavC"
        "ZrPiva9n+vRag748e+C5C/l8nrVYbfjiyl1ce7BCR1ObmE5IoT2Mq7d2d26VImtMSS1CkmSk5AIu"
        "3pZD8D4/bvjo9JkBjRFWlC1M3hRxK74AVPOAWkJZyWJhaYnp7trF/Dz5A2i7CYViEZduy2JSLtP6"
        "5UYtB0slFem0HOFXNmC3GwfyUhpVbXxYc0Axm3Huj3m01zVAuv4ngrvqI3P3qHGtLgqS5HRAIJOR"
        "+Uqlcur4gR2BhFzi8mYXa7JYkUytY03KIqvkIaSSEIoGTM9LElITMbz+VUxrBmfUpe8LPs1rtpxc"
        "FEQul1uBs7ymuds80v9yz9Rycg3fXLmhqz0LqsLBUA3D0M+hXOHh94M4fvLd4ZMnjobjcQHxBYEX"
        "xeWYqpZHL02cFx/90tj9qt6vQV01NDUokSP/CjAAqh2B+z8LHygAAAAASUVORK5CYII="
    ),
)

import wx
import asynchat
import asyncore
import socket



# DIGITAL PRESS ACTION
class DigitalPress(eg.ActionBase):
    name = "Digital Press"
    iconFile = "icon_lightblue"
    description = (
        "Fire a digital press event to a given join number."
    )
    class text:
        label = "Digital Join Number:"
  
        
    def __call__(self, joinNum):
        self.plugin.Push("d%s=1\x03" % (joinNum, ))
               
            
    def Configure(self, joinNum="1"):
        panel = eg.ConfigPanel()
        st1 = panel.StaticText(self.text.label)
        textCtrl = panel.SpinIntCtrl(joinNum, min=1, max=9999)
        mySizer = wx.FlexGridSizer(rows=3)
        mySizer.Add(st1, 0, wx.EXPAND|wx.ALL, 5)
        mySizer.Add(textCtrl, 0, wx.EXPAND|wx.ALL, 5)
        panel.sizer.Add(mySizer)
        while panel.Affirmed():
            panel.SetResult(textCtrl.GetValue())



# DIGITAL RELEASE ACTION
class DigitalRelease(eg.ActionBase):
    name = "Digital Release"
    iconFile = "icon_blue"
    description = (
        "Fire a digital release event to a given join number."
    )
    class text:
        label = "Digital Join Number:"
    
        
    def __call__(self, joinNum):
        self.plugin.Push("d%d=0\x03" % (joinNum, ))
               
            
    def Configure(self, joinNum="1"):
        panel = eg.ConfigPanel()
        st1 = panel.StaticText(self.text.label)
        textCtrl = panel.SpinIntCtrl(joinNum, min=1, max=9999)
        mySizer = wx.FlexGridSizer(rows=3)
        mySizer.Add(st1, 0, wx.EXPAND|wx.ALL, 5)
        mySizer.Add(textCtrl, 0, wx.EXPAND|wx.ALL, 5)
        panel.sizer.Add(mySizer)
        while panel.Affirmed():
            panel.SetResult(textCtrl.GetValue())



# ANALOG CHANGE ACTION
class AnalogChange(eg.ActionBase):
    name = "Analog Change"
    iconFile = "icon_red"
    description = (
        "Send a new analog value to a given join number."
    )
    class text:
        label = "Analog Join Number:"
        value = "Analog Value (0-65535):"
        minvalue = "Minimum Value:"
        maxvalue = "Maximum Value:"
        
    def __call__(self, joinNum, Value="0", minVal="0", maxVal="65535"):
        val = eg.ParseString(Value)
        val = str(round(float(65535 / float(maxVal) - float(minVal)) * float(val)))
        self.plugin.Push("a%s=%s\x03" % (joinNum, val))
               
            
    def Configure(self, joinNum="1", Value="0", minVal="0", maxVal="65535"):
        panel = eg.ConfigPanel()
        st1 = panel.StaticText(self.text.label)
        st2 = panel.StaticText(self.text.value)
        st3 = panel.StaticText(self.text.minvalue)
        st4 = panel.StaticText(self.text.maxvalue)
        eg.EqualizeWidths((st1, st2))
        valCtrl = panel.TextCtrl(Value)
        textCtrl = panel.SpinIntCtrl(joinNum, min=1, max=9999)
        minvalCtrl = panel.TextCtrl(minVal)
        maxvalCtrl = panel.TextCtrl(maxVal)
        mySizer = wx.FlexGridSizer(rows=8)
        mySizer.Add(st1, 0, wx.EXPAND|wx.ALL, 8)
        mySizer.Add(textCtrl, 0, wx.EXPAND|wx.ALL, 8)
        mySizer.Add(st2, 0, wx.EXPAND|wx.ALL, 8)
        mySizer.Add(valCtrl, 0, wx.EXPAND|wx.ALL, 8)
        mySizer.Add(st3, 0, wx.EXPAND|wx.ALL, 8)
        mySizer.Add(minvalCtrl, 0, wx.EXPAND|wx.ALL, 8)
        mySizer.Add(st4, 0, wx.EXPAND|wx.ALL, 8)
        mySizer.Add(maxvalCtrl, 0, wx.EXPAND|wx.ALL, 8)
        panel.sizer.Add(mySizer)
        while panel.Affirmed():
            panel.SetResult(textCtrl.GetValue(), valCtrl.GetValue(), minvalCtrl.GetValue(), maxvalCtrl.GetValue())
            


# SERIAL CHANGE ACTION
class SerialChange(eg.ActionBase):
    name = "Serial Change"
    iconFile = "icon_black"
    description = (
        "Send a new serial value to a given join number."
    )
    class text:
        label = "Serial Join Number:"
        value = "Serial Value:"
        
    def __call__(self, joinNum, Value):
        self.plugin.Push("s%s=%s\x03" % (joinNum, Value))
               
            
    def Configure(self, joinNum="1", Value="0"):
        panel = eg.ConfigPanel()
        st1 = panel.StaticText(self.text.label)
        st2 = panel.StaticText(self.text.value)
        eg.EqualizeWidths((st1, st2))
        valCtrl = panel.TextCtrl(Value)
        textCtrl = panel.SpinIntCtrl(joinNum, min=1, max=9999)
        mySizer = wx.FlexGridSizer(rows=5)
        mySizer.Add(st1, 0, wx.EXPAND|wx.ALL, 5)
        mySizer.Add(textCtrl, 0, wx.EXPAND|wx.ALL, 5)
        mySizer.Add(st2, 0, wx.EXPAND|wx.ALL, 5)
        mySizer.Add(valCtrl, 0, wx.EXPAND|wx.ALL, 5)
        panel.sizer.Add(mySizer)
        while panel.Affirmed():
            panel.SetResult(textCtrl.GetValue(), valCtrl.GetValue())

# SOCKET SEND ACTION
class SocketSend(eg.ActionBase):
    name = "Socket Send"
    iconFile = "icon_orange"
    description = (
        "Send a raw string value direct to the TCP socket."
    )
    class text:
        value = "String Value:"
        
    def __call__(self, Value):
        val = eg.ParseString(Value)
        self.plugin.Push("%s" % (val))
               
            
    def Configure(self, Value="l1=0x\x03"):
        panel = eg.ConfigPanel()
        st1 = panel.StaticText(self.text.value)
        valCtrl = panel.TextCtrl(Value)
        mySizer = wx.FlexGridSizer(rows=5)
        mySizer.Add(st1, 0, wx.EXPAND|wx.ALL, 5)
        mySizer.Add(valCtrl, 0, wx.EXPAND|wx.ALL, 5)
        panel.sizer.Add(mySizer)
        while panel.Affirmed():
            panel.SetResult(valCtrl.GetValue())

class ServerSession(asynchat.async_chat):
   
    def __init__ (self, sock, addr, password, plugin, server_ref):
        # Call constructor of the parent class
        asynchat.async_chat.__init__(self, sock)

        # Set up input line terminator
        self.set_terminator('\x03')

        # Initialize input data buffer
        self.data = ''
        self.plugin = plugin
        self.password = password
        self.ip = addr[0]
        self.server_ref = server_ref


    def handle_connect(self):
        # connection succeeded
        self.plugin.TriggerEvent("Connected")
         
        
    def handle_expt(self):
        # connection failed
        self.plugin.TriggerEvent("NoConnection")
        self.close()


    def handle_close(self):
        # connection closed
        self.plugin.TriggerEvent("ConnectionLost")
        self.plugin.EndLastEvent()
        asynchat.async_chat.handle_close(self)


    def collect_incoming_data(self, data):
        # received a chunk of incoming data
        self.data = self.data + data


    def found_terminator(self):
        self.ParseIncoming(self.data)
        self.data = ''


    def ParseIncoming(self, line):
        line = line.strip()
        if self.plugin.logMessages:
            print("CommandFusion Message recv: " + eg.ParseString(line))
        if line[0] == "h":
            # Heartbeat message
            try:
                self.SendData('h=1\x03')
            except Exception, exc:
                raise self.plugin.Exception(exc[1])
        elif line[0] == "p":
            # Password message
            if line[2:] == self.password or self.password == "":
                # Password correct
                self.SendData('p=ok\x03')
            else:
                # Password failed
                self.SendData('p=bad\x03')
                self.plugin.PrintError("Password failed!: " + eg.ParseString(line[2:]))
                #self.initiate_close()
        elif line[0] == "i":
            # Init request
            self.plugin.TriggerEvent("InitializeRequest")
        elif line[0] == "d":
            # Digital Join event, eg. d10=1
            equalsPos = line.find('=')
            joinNum = line[1:equalsPos]
            joinVal = line[equalsPos+1]
            if joinVal == "1":
                self.plugin.TriggerEvent("DigitalPress_%s" % (joinNum,))
            else:
                self.plugin.TriggerEvent("DigitalRelease_%s" % (joinNum,))
        elif line[0] == "a":
            # Analog Join event, eg. a2=4000
            equalsPos = line.find('=')
            joinNum = line[1:equalsPos]
            joinVal = line[equalsPos+1:]
            self.plugin.TriggerEvent("AnalogChange_%s" % (joinNum,), joinVal)
        elif line[0] == "s":
            # Serial Join event, eg. s4=this is text
            equalsPos = line.find('=')
            joinNum = line[1:equalsPos]
            joinVal = line[equalsPos+1:]
            self.plugin.TriggerEvent("SerialChange_%s" % (joinNum,), joinVal)


    def SendData(self, data):
        try:
            self.sendall(data)
        except Exception, exc:
            raise self.plugin.Exception(exc[1])



class Server(asyncore.dispatcher):
    
 
    def __init__ (self, port, password, handler):
        self.handler = handler
        self.password = password
        # Call parent class constructor explicitly
        asyncore.dispatcher.__init__(self)
        
        # Create socket of requested type
        self.create_socket(socket.AF_INET, socket.SOCK_STREAM)
        
        # restart the asyncore loop, so it notices the new socket
        eg.RestartAsyncore()

        # Set it to re-use address
        #self.set_reuse_addr()
        
        # Bind to all interfaces of this host at specified port
        self.bind(('', port))

        # Start listening for incoming requests
        self.listen(5)


    def handle_accept (self):
        """Called by asyncore engine when new connection arrives"""
        # Accept new connection
        (sock, addr) = self.accept()
        self.session = ServerSession(
            sock, 
            addr, 
            self.password, 
            self.handler, 
            self
        )



class CommandFusion(eg.PluginBase):
    
    canMultiLoad = True

    class text:
        tcpBox = "TCP/IP Settings"
        port = "Port:"
        passwordBox = "Security Settings"
        password = "Password:"
        showLogBox = "Log Settings"
        showLog = "Enable logging of all messages."
        
        
    def __init__(self):
        self.AddAction(DigitalPress)
        self.AddAction(DigitalRelease)
        self.AddAction(AnalogChange)
        self.AddAction(SerialChange)
        self.AddAction(SocketSend)

                            
    def __start__(self, port, password, logMessages):
        self.port = port
        self.password = password
        self.dispatcher = None
        self.isDispatcherRunning = False
        self.logMessages = logMessages
        try:
            self.dispatcher = Server(self.port, self.password, self)
            self.isDispatcherRunning = True
        except socket.error, exc:
            raise self.Exception(exc[1])       

    
    def __stop__(self):
        if self.dispatcher:
            self.dispatcher.close()
        self.dispatcher = None


    def Push(self, data):
        if not self.isDispatcherRunning:
            self.connectedevent = data
            self.dispatcher = Server(self.port, self.password, self)
            self.isDispatcherRunning = True
        try:
            self.dispatcher.session.sendall(data)
            if self.logMessages:
                print("CommandFusion Message sent: " + eg.ParseString(data))
            return True
        except Exception, exc:
            if self.logMessages:
                print("CommandFusion Message could not be sent: " + eg.ParseString(data))
            #raise self.Exception(exc[1])


    def Configure(self, port=8020, password="", logMessages=True): 
        text = self.text
        panel = eg.ConfigPanel()
        portCtrl = panel.SpinIntCtrl(port, max=65535)
        passwordCtrl = panel.TextCtrl(password, style=wx.TE_PASSWORD)
        checkBox = panel.CheckBox(logMessages, text.showLog)
        st1 = panel.StaticText(text.port)
        st2 = panel.StaticText(text.password)
        st3 = panel.StaticText(text.showLog)
        eg.EqualizeWidths((st1, st2, st3))
        tcpBox = panel.BoxedGroup(text.tcpBox, (st1, portCtrl))
        securityBox = panel.BoxedGroup(text.passwordBox, (st2, passwordCtrl))
        logBox = panel.BoxedGroup(text.showLogBox, (st3, checkBox))
        panel.sizer.Add(tcpBox, 0, wx.EXPAND)
        panel.sizer.Add(securityBox, 0, wx.TOP|wx.EXPAND, 10)
        panel.sizer.Add(logBox, 0, wx.TOP|wx.EXPAND, 10)
        while panel.Affirmed():
            panel.SetResult(
                portCtrl.GetValue(), 
                passwordCtrl.GetValue(),
                checkBox.GetValue()
            )

