# ARPB2 project

## Run locally without ARCore

You can run `BoardLab` scene on Unity in order to generate a fake scenario to play around with ARPB2 without the need of a phone, plane detection or server connections of any kind. Just open the scene and press Play on Unity. You will find on the top right corner a button to send some actions to ARPB2. These actions can be edited on runtime just by going to UI > HUD > ExecuteCodeButton (at the Hierarchy tab, where you can see the GameObjects of the scene), and to the ExecuteCodeButtonBehaviour component

Setting up the debugger for VSCode is easy enough. You only need to install the `Debugger for Unity` extension and that's it, you are good to go.

## Utils

### See logs

`adb.exe logcat -s Unity PackageManager dalvikvm DEBUG | findstr ">>>"`
