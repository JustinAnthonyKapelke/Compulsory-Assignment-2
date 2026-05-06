# 📚 Compulsory-Assignment-2  
## AI Research Agent Using AutoGen

An AI-powered research agent that retrieves academic papers using the OpenAlex API, applies deterministic filtering, and generates structured explanations using an LLM (Mistral).

---

# 🚀 Features

- 🔎 Search academic papers via OpenAlex API  
- 📅 Filter by publication year (min / max)  
- 📊 Filter by minimum citation count  
- 🧠 Keyword-based ranking system  
- 🤖 LLM-generated structured explanations (JSON output)  
- 🛑 Anti-hallucination design (LLM only explains retrieved paper)  
- ⚙️ Deterministic constraint enforcement in C# (not LLM-based)

---

# 🧱 Tech Stack

- 🟦 .NET (C#)
- 📡 OpenAlex API (academic paper retrieval)
- 🧠 Mistral LLM (explanations)
- 📦 Newtonsoft.Json
- 🐍 Python (backend service)

---

# ▶️ How to Run the Project

## 1️⃣ Start Python backend

Open a terminal inside the Python folder and run these installs:

Run the this command, at least version 3.11.9 is required to continue
- "python --version"

If python is not installed, run this command
- "pip install python"

Now continue with these dependencies.
- "pip install python"
- "pip install mistralai"
- "pip install fastapi"
- "pip install uvicorn"
- "pip install autogen"


Run this command to launch the server from the same terminal in the python fodler.
- "python -m uvicorn app:app --reload --port 8000"

If application startup is succesful it will inform you that uvicorn is running on http://127.0.0.1:8000 and the startup is complete

Now open a terminal on "AI Research Agent" solution and type this command to run the .net app
- "dotnet run"

If you are using an IDE, you can just run the program.

## 2️⃣ API Key

We are using Mistral AI and the API key is hardcoded in the config.py file within the solution.
We are aware, that API keys should be stored as .env files locally for security purposes, but for the sake of ease, we've chosen to hardcode it manually in the application. 

## 3️⃣ Evaluation Results

The evaluation results are uploaded as a seperate pdf file in this repository. 

The solution was developed using pair-programming, as our team consisted of only two members.
