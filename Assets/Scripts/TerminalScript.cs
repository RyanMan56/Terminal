using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class TerminalScript : MonoBehaviour {
    public Material screenOnMaterial, screenOffMaterial;

    private PlayerController playerController;
    private GameObject text, screen, player;
    private Collider screenCol;
    private Renderer screenRenderer;
    private TextMesh screenTextMesh;
    private MeshRenderer screenTextMeshRenderer;

    private string OSBeginText = "TerminOS; [Version 10.0.14393]\n(c) 2052 TermiCorp. All rights reserved.\n";
    private string currentText, typingText = "", directory = "\nC:\\>";
    private float OSTextLength, currentTextLength, timer = 0;
    public bool terminalOn;
    public int lineWidth, lineHeight, maxLineWidth, maxLineHeight;
    private List<string> line;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        screen = transform.Find("Screen").gameObject;
        screenCol = screen.GetComponent<Collider>();
        screenRenderer = screen.GetComponent<Renderer>();
        screenRenderer.material = screenOnMaterial;
        text = screen.transform.Find("Text").gameObject;
        line = new List<string>();
        screenTextMesh = text.GetComponent<TextMesh>();
        screenTextMeshRenderer = text.GetComponent<MeshRenderer>();

//        WriteToTerminal("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        WriteToTerminal(OSBeginText);
        WriteToTerminal(directory);
        //WriteToTerminal("hel\n\nlo\n");
        //WriteToTerminal("HelloWor\n");
        //WriteToTerminal("Test_Text_Test");
        //WriteToTerminal("Test_Text_Test");
        //WriteToTerminal("Hello");
	}
	
	// Update is called once per frame
	void Update () {
        currentText = "";
        int offset = 0;
        if(lineHeight > maxLineHeight)
            offset = lineHeight - maxLineHeight;
        for(int i = offset; i < line.Count-1; i++)
        {
            currentText += line[i]+'\n';
        }
        currentText += line[line.Count-1];
        screenTextMesh.text = currentText + typingText;
        if (terminalOn)
        {
            if (screenRenderer.sharedMaterial.Equals(screenOffMaterial))
            {
                screenRenderer.material = screenOnMaterial;
                screenTextMeshRenderer.enabled = true;
            }
            if (timer == 0)
            {
                if (Input.GetMouseButton(0))
                {
                    bool canMove = playerController.canMove;
                    Ray ray = Camera.main.ScreenPointToRay(new Vector2((Screen.width - 1) / 2, (Screen.height - 1) / 2));
                    RaycastHit hit;
                    bool buttonPressed = Physics.Raycast(ray, out hit, 1.5f);
                    if (buttonPressed && (hit.collider.Equals(screenCol) || hit.collider.Equals(screenCol)))
                    {
                        if (canMove)
                            playerController.canMove = false;
                        else
                            playerController.canMove = true;
                        timer = 0.5f;
                    }
                }
            }
            else
            {
                timer -= Time.deltaTime;
                if (timer < 0)
                    timer = 0;
            }

            if (!playerController.canMove)
            {
                foreach (char c in Input.inputString)
                {
                    if (c == '\b')
                    {
                        if (typingText.Length != 0)
                        {
                            typingText = typingText.Substring(0, typingText.Length - 1);
                            lineWidth--;
                            if (lineWidth <= 0)
                            {
                                lineWidth = maxLineWidth;
                                lineHeight--;
                                typingText = typingText.Substring(0, typingText.Length - 1);
                            }
                        }
                    }
                    else
                        if (c == '\n' || c == '\r')
                    {
                        typingText = typingText.Replace("\n", string.Empty);
                        typingText = typingText.Replace("\r", string.Empty);
                        WriteToTerminal(typingText + '\n');
                        CheckInput();
                        typingText = "";
                    }
                    else
                    {
                        if (lineWidth >= maxLineWidth)
                        {
                            typingText += '\n';
                            lineWidth = 0;
                            lineHeight++;
                        }
                        typingText += c;
                        lineWidth++;
                     }
                }
            }
        }
        else
        {
            if (screenRenderer.sharedMaterial.Equals(screenOnMaterial))
            {
                screenRenderer.material = screenOffMaterial;
                screenTextMeshRenderer.enabled = false;
            }
            if (!playerController.canMove)
                playerController.canMove = true;
        }
	}

    void CheckInput()
    {
        if(typingText.ToLower() == "help")
        {
            Help();
        }
        //else if (typingText == "BigBooty95")
        //{
        //    WriteToTerminal("Congrazt You Have Found Lukes Mum\n\n");
        //    for (int adamsCounter = 0; adamsCounter != 10; adamsCounter++)
        //    {
        //        WriteToTerminal("Yes Sugarlips ;)\n\n");
        //    }
        //    WriteToTerminal(directory);
        //}
        else
        {
            string noCommand = "Command not recognised, type help for a list of commands\n";
            WriteToTerminal(noCommand);// currentText += noCommand;
            WriteToTerminal(directory);// currentText += "\nC:\\>";
        }
    }

    void Help()
    {
        string helpString = "Welcome to TerminOS, no commands available\n";
        WriteToTerminal(helpString);// currentText += helpString;
        WriteToTerminal(directory);// currentText += "\nC:\\>";
    }

    void WriteToTerminal(string text)
    {
        if (lineWidth + text.Length > maxLineWidth || text.Contains('\n') || text.Contains('\r'))
        {
            int textLineWidth = text.Length, escapePos = -1, escapeCount = 0;
            while (textLineWidth > 0)
            {
                int length = 0;
                string currentLine = "";
                if (maxLineWidth - lineWidth > textLineWidth)
                    length = textLineWidth;
                else
                    length = maxLineWidth - lineWidth;
                if (line.Count - 1 < lineHeight)
                    line.Add("");
                currentLine = text.Substring(text.Length - textLineWidth + escapeCount, length - escapeCount);
                escapePos = -1;
                if (currentLine.Contains('\n'))
                    escapePos = currentLine.IndexOf('\n');
                if (currentLine.Contains('\r'))
                    if (escapePos != -1 && currentLine.IndexOf('\r') < escapePos)
                        escapePos = currentLine.IndexOf('\r');
                if (escapePos != -1)
                {
                    currentLine = currentLine.Substring(0, escapePos); // Doesn't include escape character
                    length = currentLine.Length;
                    escapeCount++;
                }
                line[lineHeight] += currentLine;
                textLineWidth -= (length);
                if (lineWidth + length >= maxLineWidth || escapePos != -1)
                {
                    lineWidth = 0;
                    lineHeight++;
                }
                else
                    lineWidth += length;
            }
        }
        else
        {
            if (line.Count - 1 < lineHeight)
                line.Add("");
            line[lineHeight] += text;
            lineWidth += text.Length;
        }
    }
}
