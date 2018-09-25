<p>
  <img align="left" src="./uofr_logo.jpg" alt="U of R logo" width="39.055%"/>
  <img align="right" src="./ehealth_logo.png" alt="eHealth logo" width="27.5%"/>
</p>

<br/><br/><br/><br/>

**Current Contributors:** Tristan Heisler, Shawn Clake, Jennifer Herasymuik, Quinn Bast, Wilson Nie, Oscar Lou
<br/>
**Original Authors:** Reid Stancu, Simranjeet Kaur, Susmita Patel

# Functional Requirements

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
**Table of Contents**

  - [1 Introduction](#1-introduction)
    - [1.1 Purpose](#11-purpose)
    - [1.2 Scope](#12-scope)
    - [1.3 References](#13-references)
    - [1.4 Assumptions](#14-assumptions)
  - [2 Methodology](#2-methodology)
  - [3 Functional Requirements](#3-functional-requirements)
      - [Table 1: Creating/Editing Ideas](#table-1-creatingediting-ideas)
      - [Table 2: Viewing Ideas](#table-2-viewing-ideas)
      - [Table 3: Searching/Filtering Ideas](#table-3-searchingfiltering-ideas)
      - [Table 4: Viewing Statistics of Ideas](#table-4-viewing-statistics-of-ideas)
      - [Table 5: Home Page/Login/Contact Us](#table-5-home-pagelogincontact-us)
      - [Table 6: PDEA Management](#table-6-pdea-management)
      - [Table 7: Administrator Tools](#table-7-administrator-tools)
      - [Table 8: User Account](#table-8-user-account)
  - [4 Other Requirements](#4-other-requirements)
    - [4.1 Interface Requirements](#41-interface-requirements)
      - [4.1.1 Software Interfaces](#411-software-interfaces)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## 1 Introduction
The system requested by eHealth with the goal to improve the quality of healthcare across the province. This follows the organization's ongoing efforts to foster an internal culture of innovation. Using Plan, Do, Evaluate, Adapt (PDEA) cycles the goal is to improve quality, cost, delivery, safety, and engagement (QCDSE).

The proposed project accomplishes this by providing an application for employees to submit their ideas for improving existing organizational processes.  This increases visibility of individual and collaborative work.

**Notice:** PDCA (**P**lan **D**o **C**heck **A**ct) has been substituted by PDEA (**P**lan **D**o **E**valuate **A**dapt). See [Discussion](./discussions.md) document.

### 1.1 Purpose
The purpose of this document is to provide a backbone reference for the development of the eIDEAS project. The below sections are intended to provide a list of specific functional requirements that can lead to a minimum viable product.

### 1.2 Scope
The scope of this document is to provide enough information to the group so that they have a good starting point for their work during Phase 2.

### 1.3 References
Refer to [UR Courses](https://urcourses.uregina.ca)  project requirement documentation.

Also refer to the [Discussion Document](./discussions.md).

### 1.4 Assumptions
 * The project will be under MIT license.
 * Balsamiq is used to creating mockup screens.
 * Vue.js will be used.
 * Source code and related documents will be hosted on a public [GitHub](https://github.com/rstancu/braintrust).

## 2 Methodology
Using Balsamiq, mockups for the envisioned project were created. Each mockup screen helps to identify a piece of the functionality for the whole project.

**TL;DR**
Take mockups and break them down into individual requirements (1 sentence each), then put them into a table.

## 3 Functional Requirements

#### Table 1: Creating/Editing Ideas
| ID     | Requirement Definition     |
| :--- | :--- |
| FR1-1 | The system shall allow the user to create a new idea.   |
| FR1-1.1 | When creating a new idea the system shall require the user to enter a title, problem description, and solution plan.   |
| FR1-1.2 | When creating a new idea the system shall require  the user (where applicable) to select a division and/or unit via an autocomplete drop down list.   |
| FR1-1.3 | The system shall allow the user to 'Save as Draft' or 'Submit' the idea.   |
| FR1-1.4 | The system shall allow the user to add amendments to a submitted idea.   |
| FR1-2 | The system shall allow the user to edit their non-submitted ideas by saving as a draft.   |

#### Table 2: Viewing Ideas
| ID     | Requirement Definition     |
| :--- | :--- |
| FR2-1 | The system shall allow the user to view their own (as well as others) ideas.   |
| FR2-1.1 | Each idea shall display (at minimum) the following information: idea score, submitter name, team, idea title, idea description, ~~"tags"/affecting areas~~, idea creation time, PDEA status.   |
| FR2-2 | The system shall allow the user to view the PDEA status of a current idea.   |

#### Table 3: Searching/Filtering Ideas
| ID     | Requirement Definition     |
| :--- | :--- |
| FR3-1 | The system shall allow the user to view successful (completed) ideas from the past.   |
| FR3-2 | The system shall allow the user to view unsuccessful (abandoned) ideas from the past.   |
| FR3-3 | The system shall allow the user to filter ideas on any field from FR2-1.1.   |
| FR3-4 | The system shall allow the user to filter ideas on some values that are not visible (e.g. Division, idea id, creation date).   |

#### Table 4: Viewing Statistics of Ideas
| ID     | Requirement Definition     |
| :--- | :--- |
| FR4-1 | The system shall allow the user to view the total number of (global) submitted ideas.   |
| FR4-2 | The system shall allow the user to view an 'Idea Points vs Time' plot for teams and individuals.   |
| FR4-3 | The system shall allow the user to view a number of different statistical charts to visualize points data.   |
| FR4-4 | The system shall provide a 'Goto Me' button that when pressed automatically displays the users individual statistics.   |
| FR4-5 | The system shall provide a 'Goto My Team' button that when pressed automatically displays the users teams' statistics. (This may have to be disabled because a user may or may not belong to more than one team)  |

#### Table 5: Home Page/Login/Contact Us
| ID     | Requirement Definition     |
| :--- | :--- |
| FR5-1 | The system shall allow the user to log into their eIDEAS account.   |
| FR5-1.1 | The system shall allow the user to create an eIDEAS account.   |
| FR5-1.2 | The system shall allow the user to authenticate with their eIDEAS username and password.   |
| FR5-1.3 | The system shall display a "forgot password" link closely in proximity to the login button.   |
| FR5-1.3.1 | The system shall take the user to a forgot password page after clicking the "forgot password" link.   |
| FR5-2 | The system shall have a home page presented after logging in.   |
| FR5-2.1 | The system's home page shall have a tabular navigation bar.   |
| FR5-2.2 | The system's home page shall display graphical 2D statistics about ideas (see FR4-1.1).   |
| FR5-2.3 | The system's home page shall display a small paragraph about eIDEAS (via pop-up on the homepage).   |
| FR5-2.4 | The system shall have a "Contact Us" page link in the tabular navigation.   |
| FR5-2.4.1 | The "Contact Us" page shall display name, email, message field, and a submit button.   |

#### Table 6: PDEA Management
| ID     | Requirement Definition     |
| :--- | :--- |
| FR6-1 | The system shall allow users to track ideas. Ideas that are marked as "tracked" must appear in the **Tracked Ideas** tab.   |
| FR6-2 | The system shall allow users to untrack ideas. Ideas that are untracked must be removed from the **Tracked Ideas** tab.   |
| FR6-3 | The system shall display the following for tracked ideas: idea id, submitter name, team, idea title, current PDEA status (via drop-down selection), 'GO' button.   |
| FR6-3.1 | Double-clicking an idea in the **Tracked Ideas** tab shall display the idea's expanded view as defined by FR2-1.1 via pop-up.   |
| FR6-3.2 | The **Tracked Ideas** tab shall allow the user to change the PDEA status of an idea for their respective team(s).   |
| FR6-3.3 | The system shall provide a drop-down menu to select Plan/Do/Evaluate/Adapt/Abandon/Complete statuses for an idea.   |
| FR6-3.3.1 | The drop-down menu shall enforce the correct PDEA flow (e.g. P->D->E->A).   |
| FR6-3.3.2 | The 'Complete' status shall only be available after the 'Evaluate' phase.   |
| FR6-3.3.3 | The 'Abandon' status shall be available at any phase.   |
| FR6-4 | After selection of each Plan/Do/Evaluate/Adapt/Abandon/Complete status a pop-up confirmation message will be presented after the user clicks the 'GO' button.   |
| FR6-5 | After selecting the Adapt status the system shall require the user to type in an updated plan. This plan should reflect adaptations discovered in the Evaluation phase.   |
| FR6-6 | After selecting the Abandon status the system shall require the user to type in a reason for abandonment.   |

#### Table 7: Administrator Tools
| ID     | Requirement Definition     |
| :--- | :--- |
| FR7-1 | The system shall allow an Administrator to Add/Edit/Delete users.   |
| FR7-2 | The system shall allow an Administrator to change the PDEA status of ideas.   |
| FR7-3 | The system shall allow an Administrator to Delete ideas.   |
| FR7-4 | The system shall allow an Administrator to Add/Rename/Delete Divisions and Units.   |
| FR7-5 | The system shall allow an Administrator to manage the points system (see [Discussion](./discussions.md) document for details).   |
| FR7-6 | The system shall allow an Administrator to manage rewards.   |
| FR7-6.1 | The system shall allow an Administrator to manage Global and Team rewards (see [Discussion](./discussions.md) document for details).   |
| FR7-7 | The system shall allow an Administrator to manage the homepage *Success Stories* and *What's New*.   |

#### Table 8: User Account
| ID     | Requirement Definition     |
| :--- | :--- |
| FR8-1 | The system shall allow the user to change their account information (i.e email, password, phone number, etc).   |
| FR8-2 | The system shall allow the user to upload a profile photo.   |
| FR8-3 | The system shall allow the user to view their current reward progress (both team and individual).   |

## 4 Other Requirements
The application should be "user-friendly" and easy to maintain.

### 4.1 Interface Requirements
The interface should be web based (Vue.js) and mobile device friendly.

#### 4.1.1 Software Interfaces
Some kind of web stack (e.g. LAMP), using Vue.js.
