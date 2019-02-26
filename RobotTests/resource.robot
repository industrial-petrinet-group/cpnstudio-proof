*** Settings ***
Library    String
Library    WhiteLibrary

*** Variables ***IPG.CPNStudio.Demo\bin\Debug
${TEST APPLICATION}      ${EXECDIR}${/}IPG.CPNStudio.Demo${/}bin${/}Debug${/}IPG.CPNStudio.Demo.exe
${TEST APPLICATION NAME}    MainWindow

*** Keywords ***

Launch Application For Test
    [Arguments]    ${args}=${EMPTY}
    Set Log Level    Info
    Launch Application    ${TEST APPLICATION}    ${args}
    Attach Window    MainWindow

Attach Main Window
    Attach Window    MainWindow

Launch White Application For Test
    Set Log Level    Info
    Launch Application    ${TEST WHITE APPLICATION}
    Attach Window    ${TEST WHITE APPLICATION NAME}

Attach White Main Window
    Attach Window    ${TEST WHITE APPLICATION NAME}

Clean Application
    Input Text To Textbox    txtA    ${EMPTY}
    Input Text To Textbox    txtB    ${EMPTY}
    Select Combobox Index    op    0
    Input Text To Textbox    tbResult    ${EMPTY}
    Click Button    progressResetBtn
    Select Radio Button    rb_peke

Setup For Tab 2 Tests
    Attach Main Window
    Select Tab Page    tabControl    Tab2

${node label} Should Be ${status}
    [Documentation]    Note that node label is case sensitive
    ${status}=    Convert To Lowercase    ${status}
    Verify Label    selectionIndicatorLabel    ${node label} ${status}
