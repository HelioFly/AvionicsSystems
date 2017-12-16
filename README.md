# AvionicsSystems
MOARdV's Avionics Systems for Kerbal Space Program - a new generation of IVA enhancement.

## Short Intro

This is a project to rebuild RasterPropMonitor
from the ground up using the design techniques I retrofitted to RPM during the first half
of 2016.  The goal is to provide a leaner, more performant design without sacrificing
features that are available in RPM.  A longer-winded explanation is available in the
wiki's [Introduction](https://github.com/MOARdV/AvionicsSystems/wiki/Introduction).

MAS is under intermittent development (basically, when I feel like it and I have time).

The [wiki](https://github.com/MOARdV/AvionicsSystems/wiki) contains most of the documentation for this mod, including guides to
configuring props and integrating scripting.

Releases appear under [Releases](https://github.com/MOARdV/AvionicsSystems/releases).  Development builds
occasionally show up on DropBox, and they may be mentioned in the [MAS Beta Release Announcements](https://github.com/MOARdV/AvionicsSystems/issues/95) issue.

## Where are the IVAs?

MAS is intended to be a toolkit for developing IVAs.  I do not intend to include a wide range of example
IVAs like RPM had - good IVAs take a lot of time to create, and I am not packaging stock-derived sample
props with this mod.  My expectation is that
IVAs using MAS will include at least one of alexustas's props packs - [ASET Props Pack](http://forum.kerbalspaceprogram.com/index.php?/topic/116430-aset-props-pack-v14-for-the-modders-who-create-iva/) and/or
[ASET Avionics Pack](http://forum.kerbalspaceprogram.com/index.php?/topic/116479-aset-avionics-pack-v-20-for-the-modders-who-create-iva/) adapted to MAS.

However, since it is important to have *something* to look at before deciding to adopt this mod for IVA
development, there is one IVA included in this package.

Please understand that the IVA is under development, and it may be missing some features.
It is the command pod I use almost excluively in my own games.

The included IVA is the Yarbrough08 Mk. 1-1 A2 Command Pod from [Yarbrough08](http://forum.kerbalspaceprogram.com/index.php?/topic/88604-wip-105-2-kerbal-command-pod-mk-1-1-a2-alpha-04-spacedock/).
Since the original mod has not been updated since KSP 1.0.5, I have included a MM patch in the MOARdV/FlightSystems directory
called YarbroughMk1-1A2_Update.cfg.  This file updates the command pod to KSP 1.3.1 standards, and it makes a few tweaks
based on my personal gameplay preferences.  **NOTE:** The career mode values (cost, tech tree placement) are guesses, since
I never play career mode.  Feedback on whether they're reasonable would be appreciated.

You will need to download and install [Module Manager](https://forum.kerbalspaceprogram.com/index.php?/topic/50533-130-module-manager-281-june-29th-2017-with-n-cats-physics/), the Yarbrough08 command pod, and
ASET Props Pack v1.5 (or later) in order to fly this IVA.

## What about RPM?

RasterPropMonitor is in maintenance mode.  I will keep RPM compiling, and I will try to fix any
egregious bugs, but I do not plan to continue developing features for the mod.  It is far too
complex for me to test it adequately by myself in a reasonable time while working on another,
substantially similar mod.  As long as RPM is still
in wide use, I will strive to keep it working, but I can not guarantee much beyond that.

IVAs designed for RPM will still work with RPM.  Props packs designed for RPM will still work with RPM.

## Other questions?

Look at the [FAQ](https://github.com/MOARdV/AvionicsSystems/wiki/FAQ) on the wiki.  I may have already come up with an answer.  If not, please ask.
