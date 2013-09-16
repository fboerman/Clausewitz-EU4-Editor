Clausewitz-EU4-Editor
=====================

This is a mod editor for the grand strategy game Europa Universalis 4 made by Paradox Interactive. This editor will first focus on country editing but later on editing for ideas, cultures and religions will be added.
The editor is written in C# and available under the GPL v2 license. You need the .net framework 4.5 in order to run it.

Current version is 0.3. you can read, edit and save all country properties. the national ideas editor cant save yet but does display the idea properties.

Instructions
------------
- Click load game and select the game folder
- Select a country and click load country
- Adjust properties to something you want (or leave everything default if you want to see if the editor creates proper mod files)
- Click save country
- Repeat for other countries
- Click save mod and give your mod a name and specify a save location
- all done!

Planning
--------

Currently available:
* country reading with all of its variables except for historical names
* seeing all the possibilites of governments\religions\cultures\techgroups and search through them in group by group.
* saving the country info to modfiles

In planning: (aka comming soon(c))
* saving and creating new national ideas/ national ideas group.
* choose wich gamefiles you want to load from basegame and wich from an existing mod.
* flag info control.
* creating new country wizard (i will try to include localization and flags for this).
* adding editors for governments, cultures, religions and techgroups (and maybe units).
* splitting all the gui logic and the actual editor program in different classes for better readability.


Changelog
---------

v0.3:
* bugfix: now correctly removes all old tag references in the ideafile
* you now get a nice popup when saving is done

v0.2:
* saving is now possible for countries. nationalideaseditor is not available in savemode yet
* added control for prefered religion. this is an optional property and not every country has this one set (wich is why i forgot it in the previous version)
* multiple bug fixes and code improvements

v0.1:
* first release, can only read properties from countries and the nationalideas

Feedback
--------
Feedback is very much appriciated. This is a learning project for me and my code is far from optimal. If you have any feedback (this includes bug reports and ideas for the program) then dont hesitate to shoot me an email at williewonka341@gmail.com. Thx in advance for your feedback!

Credit
------
Credit for the design goes to asiom.

Disclaimer
----------
This is a work in progress and a learning project for myself. As such the code is propably not 100% optimal. Im not responsible for broken mods and recommend that you save your work before you make any adjustments.