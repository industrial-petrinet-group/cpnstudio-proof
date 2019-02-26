*** Settings ***
Library    WhiteLibrary
Library    Process
Resource    resource.robot

*** Test Cases ***
Simple Test
    Attach Application By Name  IPG.CPNStudio.Demo
    Attach Window  MainWindow
    Sleep  5
    Click Button  LayoutButton 
    $item1 = Get Item  text:item1
    
    # Close Application


