# Decision Making Under Uncertainty (C#)

This C# console application demonstrates **decision making under uncertainty** using classical decision criteria such as **Maximin, Maximax, Laplace, Minimax Regret (Savage) and Hurwicz**.

The program reads a decision matrix from user input, calculates the result for each criterion, and prints which alternative (A1, A2, …) is optimal under that rule.

---

# Features
- Implements **five major decision criteria** under uncertainty  
  (Maximin, Maximax, Laplace, Hurwicz, Savage)
- **User-friendly input** for alternatives and states  
- Automatically handles **input validation**
- Works directly from the **terminal / console**
- Fully written in **C#**, no external dependencies

---

# How to Run
1. Clone the repository:
   ```bash
   git clone https://github.com/haticepolatt/decision-making-under-uncertainty.git
   
2.  Navigate to the project folder:
```
cd decision-making-under-uncertainty
```
4. Run the program:

dotnet run


## Decision Criteria Implemented 
**Maximin:** Selects the alternative with the best of the worst outcomes.

**Maximax:** Selects the alternative with the best of the best outcomes.

**Laplace:** Averages all possible payoffs for each alternative.

**Minimax Regret (Savage):** Minimizes the maximum regret value.

**Hurwicz:** Uses an optimism coefficient (α) between 0 and 1 to balance between best and worst payoffs.

## Developer
**Hatice Polat**

🎓 Akdeniz University — Management Information Systems

## License
This project is shared for educational and portfolio purposes.
