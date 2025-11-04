# SpectRA — Augmented Reality Campus Explorer

**SpectRA** is an augmented reality (AR) mobile application designed to enhance the way students, visitors, and staff interact with the university campus.  
By leveraging **Unity with AR Foundation**, **Firebase Cloud**, and **Google ARCore Services**, SpectRA delivers an immersive, real-time guided tour experience.

Users can recognize campus buildings and points of interest through their mobile device’s camera, view interactive 3D models, access historical and functional information, and receive AR-based navigation guidance.  

The project follows an **Agile Scrum methodology** to ensure iterative development, continuous feedback, and alignment with user needs.

---

##  Table of Contents
0. [Team](#team)
1. [Overview](#overview)
2. [Features](#features)
3. [Architecture Summary](#architecture-summary)
4. [Technologies Used](#technologies-used)
5. [Installation & Local Setup](#installation--local-setup)
6. [Deployment](#deployment)
7. [Documentation](#documentation)
8. [License](#license)

---

## Team

| **Name**                      | **Institutional Email**        | **Role(s)**                          |
|------------------------------|--------------------------------|--------------------------------------|
| Jean Carlo Londoño Ocampo    | jclondonoo@eafit.edu.co        | Scrum Master & Architect             |
| Alejandro Garcés Ramírez     | agarcesr@eafit.edu.co          | Developer                            |
| Tomás Londoño Lopera         | tlondonol1@eafit.edu.co        | Developer                            |
| Emanuel Gonzales Quintero    | egonzalezq@eafit.edu.co        | Product Designer & Tester            |
| Daniel Zapata Acevedo        | dzapataa3@eafit.edu.co         | UX Designer                          |

---

##  Overview

- **Platforms:** Android (min SDK 30) and iOS (min 13.0)  
- **Development Framework:** Unity’s AR Foundation abstraction layer  
- **Core Services:** ARCore (Android) and ARKit (iOS)  
- **Goal:** Enhance user engagement and accessibility through AR-driven navigation and information.

---

## Features

-  Real-time object and building recognition through AR
-  Informative overlays
-  Multimedia integration (audio, images, videos)
-  Offline functionality for basic exploration
-  Iterative updates following Agile Sprints and usability testing

---

## Architecture Summary

SpectRA integrates several key components:

| Layer | Technology | Purpose |
|-------|-------------|----------|
| **Frontend (App)** | Unity + AR Foundation | Rendering, AR logic, UI/UX |
| **Backend (Cloud)** | Firebase Cloud Firestore | Storing campus data and assets |
| **AR Services** | ARCore / ARKit | Real-time tracking and environment mapping |
| **Hosting** | Cloud web service | Distribution and version control |

---

## Technologies Used

- **Unity 2022.3 LTS**  
- **C#** for core logic  
- **AR Foundation**, **ARCore**, **ARKit**  
- **HTML5 / CSS3 & AWS** (for deployment website)  
- **GitHub Wiki** for documentation management  

---

## Installation & Local Setup

To run SpectRA locally for development or testing:

1. **Clone this repository:**
   ```bash
   git clone https://github.com/JeanCarloLondo/SpectRA.git
   cd SpectRA
    ```
2. **Open in Unity:**

- Launch Unity Hub

- Click Open Project → Select the cloned folder

- Ensure your Unity version matches 2022.3 LTS

**Set Build Target:**

- Go to File → Build Settings

- Select Android 

- Click Switch Platform

**Build and Run:**

- Connect your mobile device

- Click Build and Run to deploy the app

  ---

# Deployment

**Step 1 — Access the Web Portal**

Visit the official SpectRA deployment page hosted on our public cloud service:
https://spectra-app.cloud-demo.link

**(Provisional link)**

**Step 2 — Download the Application**

On the web page, users will find a clear **“Download SpectRA”** button.
Clicking it will automatically download the latest stable version of the app for their operating system (Android by default).

**Step 3 — Launch SpectRA**

**Once downloaded:**

- Locate the file SpectRA_Setup.exe in your downloads folder.

- Run the installer and follow the on-screen instructions.

- After installation, open the app and enjoy full offline and online functionality.

**Accessibility**

- The website and download link are publicly accessible from any internet-connected device.

- No authentication is required to access the download.

- The cloud-hosted site ensures high availability and low latency.

**Deployment Type and Reliability**

**This deployment follows a manual deployment model:**

- The web page and downloadable build are manually updated with each stable release.

- All Unity builds are verified before publishing, ensuring security and integrity.

- The process is resilient to errors, as each update is version-controlled and backed up in cloud storage.

---

# Documentation

# SPRINTS
[Sprint 0: Product Definition](https://github.com/JeanCarloLondo/SpectRA/wiki/Sprint-0-%E2%80%90-Product-Definition)

[Sprint 1 - MVP](https://github.com/JeanCarloLondo/SpectRA/wiki/Sprint-1-%E2%80%90-MVP)

[Sprint 2 - MVP V2](https://github.com/JeanCarloLondo/SpectRA/wiki/Sprint-2-%E2%80%90-MVP-V2)

[Sprint 3 - Final Product](https://github.com/JeanCarloLondo/SpectRA/wiki/Sprint-3-%E2%80%90-Final-Product)

# CEREMONIES
- [Week 2](https://github.com/JeanCarloLondo/SpectRA/wiki/CEREMONIES#week-2-contact-with-the-challenge-supervisor-via-microsoft-teams)
- [Week 3](https://github.com/JeanCarloLondo/SpectRA/wiki/CEREMONIES#week-3-virtual-meeting-with-the-challenge-supervisor)
- [Week 4](https://github.com/JeanCarloLondo/SpectRA/wiki/CEREMONIES#week-4-reflection-after-the-presentation)
- [Week 5](https://github.com/JeanCarloLondo/SpectRA/wiki/CEREMONIES#week-5-a-short-weekly-due-to-exams)
- [Week 6](https://github.com/JeanCarloLondo/SpectRA/wiki/CEREMONIES#week-6-priorities-from-other-courses)
- [Week 7](https://github.com/JeanCarloLondo/SpectRA/wiki/CEREMONIES#week-7-beginning-the-development-phase)
- [Week 8](https://github.com/JeanCarloLondo/SpectRA/wiki/CEREMONIES#week-8-sprint-delivery-delayed)
- [Week 9](https://github.com/JeanCarloLondo/SpectRA/wiki/CEREMONIES#week-9-preparing-the-mvp-presentation)
- [Week 9](https://github.com/JeanCarloLondo/SpectRA/wiki/CEREMONIES#week-9-preparing-the-mvp-presentation)
- [Week 10](https://github.com/JeanCarloLondo/SpectRA/wiki/CEREMONIES#week-10-reflecting-on-sprint-1-feedback)
- [Week 11](https://github.com/JeanCarloLondo/SpectRA/wiki/CEREMONIES#week-11-sprint-2-planning-and-task-assignment)
- [Week 12](https://github.com/JeanCarloLondo/SpectRA/wiki/CEREMONIES#week-12-a-quiet-week-with-academic-priorities)
- [Week 13](https://github.com/JeanCarloLondo/SpectRA/wiki/CEREMONIES#week-13-final-preparations-and-demo-readiness)

# License

© 2025 SpectRA Team — All Rights Reserved.
Unauthorized copying, distribution, or modification of this software or its assets is strictly prohibited.
