-------------------------------------------------------------------------------------------------
Student Details: 
-------------------------------------------------------------------------------------------------
Team:
1.	Chrishan Christesious Aloysious 101268207
2.	Vanessa Ying Xhien Wong 101951824

-------------------------------------------------------------------------------------------------
Features / Bugs / Missing
-------------------------------------------------------------------------------------------------
Features: 
A. Horn-Form
- Handles invalid file name and method name
- Handles invalid propositional symbols (Test8)
- Connectives for horn-base can be ^ and & (Test7)
B. Research Components
- can handle generic knowledge base.
- can only work for truth table
Bugs: 
A. Horn-Form
- None-known.
B. Research Components
-Test2.txt is not passeed.
-Test11.txt returns false.

Missing: 
All required features are implemented.

-------------------------------------------------------------------------------------------------
Test Cases: 
-------------------------------------------------------------------------------------------------
We have 6 test cases in total to test the program.

Test1: TELL p2=> p3; p3 => p1; c => e; b&e => f; f&g => h; p1=>d; p1&p3 => c; a; b; p2; ASK d

Test2: TELL x; x&y&i => z1; y; z2=> z3; i; i=>j; j=>z2; z3=>k; k=> p; z2&k&j&i=> q; r&p=> s; ASK s

Test3: TELL p;q;r;s; p=>t; q=>e; n&o=>t; t&e=>u; u&p=>n; s=>o; ASK t

Test4: TELL L => C; P&C=>E; E=> PZ;L;P; ASK PZ

Test5: TELL a=>b; c&d=>e; g&c=>a;f&a=>d; f&g=>c; f;g; ASK d

Test6: TELL p1&p2&p3=> p4; p5&p6 => p4; p1 => p2; p1&p2 => p3; p5&p7 => p6; p1; p4; ASK p6

Test7: p;q;r;s; p=>t; q=>e; n^o=>t; t^e=>u; u&p=>n; s=>o; ASK t

Test8: TELL p;q;r;s; p=>t; q=>e; n^o=>t; t^e=>u; u&p=>n; s=>o; ASK a

Test9: TELL p2=> p3; ASK p3

Test10: TELL p2=>p3; p1&p3=> p4; p1; p3; p4; ASK p2

Test11: TELL(a <=> (c => ~d)) & b & (b => a); c; ~f || g; (example: Generic)

-------------------------------------------------------------------------------------------------
Acknowledgements/Resources
-------------------------------------------------------------------------------------------------
1. Lecture slide 6&7
	Basic knowledge of propositional logic and knowledge base
2. https://snipplr.com/view/56296/ai-forward-chaining-implementation-for-propositional-logic-horn-form-knowledge-bases/ 
	Forward Chaining example in java code.
3. https://www.ismll.uni-hildesheim.de/lehre/ai-07s/skript/ai-4up-04-propositional-logic.pdf 
	Online lecture slide for TT, FC and BC.
4. http://people.cs.pitt.edu/~milos/courses/cs1571-Fall2012/Lectures/Class10.pdf 
	Online lecture slide for detailed TT.
5. https://www.tutorialsteacher.com/csharp 
	c# knowledge
6. https://www.youtube.com/user/fiacobelli/videos
	Videos explaining forward and backward chaining.
-------------------------------------------------------------------------------------------------
Notes
-------------------------------------------------------------------------------------------------
1. test files must be in the debug file.
2. to run the application of this program:
	- change directory: cd Assignment2/InferenceEngine/InferenceEngine/bin/Debug/
	- run program: InferenceEngine.exe <method> <testfile name>
3. Algorithms for Inference Engine availabe:
	----------------+--------------------
	Parameter	|Method Name
	----------------+--------------------
	TT              | Truth Table
	FC		| Forward Chaining
	BC		| Backward Chaining
	----------------+--------------------
4. to run the application of research component: (*test files must be in the debug file.)
	- change directory: cd Assignment2/InferenceEngine/ResearchComponent/ResearchComponent/bin/Debug/
	- run program: InferenceEngine.exe <method> <testfile name>

5. Algorithms for Research Component availabe:
	----------------+--------------------
	Parameter	|Method Name
	----------------+--------------------
	GTT             | Truth Table
	----------------+--------------------
-------------------------------------------------------------------------------------------------
Summary Report
-------------------------------------------------------------------------------------------------
Contributions: This assignment is done by Chrishan and Vanessa. 
Teamwork is empahsized, no members has overworked or underworked.
	----------------+--------------------+-------------------
	Parameter	|Chrishan            |Vanessa
	----------------+--------------------+--------------------
	TT              | 50%                | 50%
	FC		| 40%                | 60%
	BC		| 60%                | 40%	
	InferenceEngine | 50%                | 50%
	SearchMethod    | 0%                 | 100%
	Clause          | 60%                | 40%
	Program         | 100%               | 0%
        Reasearch       | 50%                | 50%
	----------------+--------------------+--------------------

Program: This is the entry of the program.

Inference engine: It receives the file data and generate the knowledge base with the help of clause class.

Clause: It helps to populate the data into a clause with head and body. 

SearchMethod: This is a abstract parent class for all the inherited algorithm classes.

TT: Truth table will be genrated for each unique propositional symbol and the model that KB entails query will be counted.
    The output is returned as YES/NO followed by the number of models.

FC: FC uses the initally known as true propositional symbols to reduce the number of unknown symbols in the premise. 
    More symbols will be known through the process. When one of the clauses has zero unknown propositional symbols,
    FC checks if the head of the clauses is the query. If yes, it returns all the symbols that are true.
    FC will stop seraching when all the known symbols have been discovered but none of them can match the query.

BC: BC is a goal driven algorithm. It uses the goal to find the root. 
    It first finds all the always true propositional symbols and checks if the query exists with in them. 
    Then, it finds the head of clause which matches the query. From the goal clause, 
    BC identifies the all the unknown symbols from the body of the clause. It repeats until it finds to the root.
    Then, it returns the symbols from root to goal. However, BC checks if the same clause occures again, if yes, 
    the program will go to infinite loop, BC stops here.