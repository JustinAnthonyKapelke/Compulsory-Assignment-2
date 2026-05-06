from fastapi import FastAPI
from pydantic import BaseModel
from typing import Optional
from agent import run_research

app = FastAPI()

class PaperRequest(BaseModel):
    query: str
    constraints: Optional[dict] = None
    papers: list


@app.post("/research")
def research(payload: PaperRequest):
    return run_research(
        query=payload.query,
        papers=payload.papers,
        constraints=payload.constraints
    )