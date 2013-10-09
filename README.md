Clausewitz-EU4-Editor
=====================

This is a mod editor for the grand strategy game Europa Universalis 4 made by Paradox Interactive. This editor will first focuses on country editing with subeditors available for culture, government, ideas etc editing.
The editor is written in C# and available under the GPL v2 license. You need the .net framework 4.5 in order to run it.

Current version is 0.4.

you can read, edit and save all country,government,ideas,religions and techgroup properties.
Version 0.4 is a rework of a lot of features, the code is a lot more efficient now.

Instructions
------------
- Click load game and select the game folder

country modding:
- Select a country and click load country
- Adjust properties to something you want (or leave everything default if you want to see if the editor creates proper mod files)
- Click save country
- Repeat for other countries

subeditors:
- click the edit button after choosing a group in the dropdown menu
- use the dropdown menus to select a propertie block to edit. if the bottom activates then there the top one is a block and you can make a subselection with the second one.
- you dont have to select a propertie, but if you start writing for the top block then that whole block is overwritten
- click save
- click exit

final modmaking:
- Click save mod and give your mod a name and specify a save location
- all done!

Planning
--------

Currently available:
* country reading with all of its variables except for historical names
* seeing all the possibilites of governments\religions\cultures\techgroups and search through them in group by group.
* editing governments, religions, cultures, techgroups and ideas
* saving the mod to a location

In planning: (aka comming soon(c))
* choose wich gamefiles you want to load from basegame and wich from an existing mod.
* flag info control.
* creating wizard to create new cultures, governments, ideas etc
* editor for groups to edit the names of for example the national ideas. this will include localization.


Changelog
---------

v0.4:
* huge rework of almost all the systems, the code is now modulare and way more efficient.
* subeditors now available! viewing editing and saving is possible
* prefered religion control added
* added extra slot for a historical idea because some countries have 9

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