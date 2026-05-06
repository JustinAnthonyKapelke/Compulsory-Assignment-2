from autogen import AssistantAgent, UserProxyAgent
from config import LLM_CONFIG
import json


# RANKER AGENT

ranker = AssistantAgent(
    name="ranker",
    llm_config=LLM_CONFIG,
    system_message="""
You are a scientific paper ranker.

TASK:
- Choose the SINGLE best paper from the list
- Base decision on relevance, citations, and quality
- Do NOT hallucinate new data

OUTPUT MUST BE VALID JSON ONLY:

{
  "title": "",
  "authors": "",
  "year": 0,
  "citations": 0,
  "url": "",
  "summary": ""
}
"""
)

# CRITIC AGENT

critic = AssistantAgent(
    name="critic",
    llm_config=LLM_CONFIG,
    system_message="""
You are a scientific paper critic and validator.

TASK:
- Review the selected paper
- Ensure it is the best choice
- Improve summary if needed
- DO NOT change factual metadata unless incorrect

STRICT OUTPUT FORMAT (ONLY JSON):

{
  "title": "",
  "authors": "",
  "year": 0,
  "citations": 0,
  "url": "",
  "summary": ""
}

RULES:
- NO extra text
- NO explanations
- NO "final_reason"
- NO "confidence"
"""
)


#USER PROXY

user = UserProxyAgent(
    name="user",
    human_input_mode="NEVER",
    code_execution_config=False
)


#MAIN PIPELINE

def run_research(query: str, constraints=None, papers=None):

   
    # STEP 1: Rank papers
   
    rank_prompt = f"""
User Query:
{query}

Constraints:
{json.dumps(constraints)}

Available Papers:
{json.dumps(papers, indent=2)}
"""

    rank_result = user.initiate_chat(
        ranker,
        message=rank_prompt,
        max_turns=1
    )

    raw_rank = rank_result.chat_history[-1]["content"]

    try:
        ranked = json.loads(raw_rank)
    except:
        ranked = {
            "title": "",
            "authors": "",
            "year": 0,
            "citations": 0,
            "url": "",
            "summary": raw_rank
        }

  
    # STEP 2: Critic validates
   
    critic_prompt = f"""
Validate and improve this selected paper:

{json.dumps(ranked, indent=2)}
"""

    critic_result = user.initiate_chat(
        critic,
        message=critic_prompt,
        max_turns=1
    )


    raw_final = critic_result.chat_history[-1]["content"]

    # STEP 3: SAFE PARSING    
    try:
        parsed = json.loads(raw_final)

        return json.dumps({
            "title": parsed.get("title", ""),
            "authors": parsed.get("authors", ""),
            "year": parsed.get("year", 0),
            "citations": parsed.get("citations", 0),
            "url": parsed.get("url", ""),
            "summary": parsed.get("summary", "")
        })

    except Exception:
        return json.dumps({
            "title": "",
            "authors": "",
            "year": 0,
            "citations": 0,
            "url": "",
            "summary": raw_final
        })