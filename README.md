# IdleMate - Keep your connection and session alive!

I needed a small application (one that wouldnt be picked up potentially as a "MouseJiggler" by endpoint detection.
It is not intended as a "Mouse Jiggler". Please use this responsibly.

The reason for this was to use in conjunction with an RDP session, so that the remote end doesnt sleep and the RDP session doesnt time out.

Quick and dirtly built in VB.NET (8), Runs in a console window.

There are no options or flags needing to be set, just run the application.
It will check system idle and create input on an idle event every 5 seconds. (Maybe a bit overkill, but it works for me)
