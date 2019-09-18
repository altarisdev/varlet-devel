; by Aris Ripandi - 2019

#define BasePath      "..\"

#define AppVersion    "1.0"
#define AppName       "Varlet Core"
#define AppPublisher  "Aris Ripandi"
#define AppWebsite    "https://arisio.us"
#define AppGithubUrl  "https://github.com/riipandi/varlet-core"
#define SetupFileName "varlet-core-1.0"

[Setup]
AppName                    = {#AppName}
AppVersion                 = {#AppVersion}
AppPublisher               = {#AppPublisher}
AppPublisherURL            = {#AppWebsite}
AppSupportURL              = {#AppWebsite}
AppUpdatesURL              = {#AppWebsite}
DefaultGroupName           = {#AppName}
OutputBaseFilename         = {#SetupFileName}
AppCopyright               = Copyright (c) {#AppPublisher}
ArchitecturesAllowed            = x64
ArchitecturesInstallIn64BitMode = x64
Compression                = lzma2/max
SolidCompression           = yes
DisableStartupPrompt       = yes
DisableWelcomePage         = no
DisableDirPage             = no
DisableProgramGroupPage    = yes
DisableReadyPage           = no
DisableFinishedPage        = no
AppendDefaultDirName       = yes
AlwaysShowComponentsList   = no
FlatComponentsList         = yes

SetupIconFile         = "setup-icon.ico"
LicenseFile           = "varlet-license.txt"
WizardImageFile       = "setup-img-side.bmp"
WizardSmallImageFile  = "setup-img-top.bmp"
DefaultDirName        = {sd}\Varlet\core
UninstallFilesDir     = {app}
Uninstallable         = yes
CreateUninstallRegKey = yes
DirExistsWarning      = yes
AlwaysRestart         = no
OutputDir             = {#BasePath}output

[Registry]
Root: HKLM; Subkey: "Software\{#AppPublisher}"; Flags: uninsdeletekeyifempty;
Root: HKLM; Subkey: "Software\{#AppPublisher}\{#AppName}"; Flags: uninsdeletekey;
Root: HKLM; Subkey: "Software\{#AppPublisher}\{#AppName}"; ValueType: string; ValueName: "InstallPath"; ValueData: "{app}";
Root: HKLM; Subkey: "Software\{#AppPublisher}\{#AppName}"; ValueType: string; ValueName: "AppVersion"; ValueData: "{#AppVersion}";

[Tasks]
Name: task_add_path_envars; Description: "Add PATH environment variables";
Name: task_install_vcredis; Description: "Install Visual C++ Redistributable";

[Files]
; Main project files ----------------------------------------------------------------------------------
Source: varlet-license.txt; DestDir: {app}; Flags: ignoreversion
Source: {#BasePath}stubs\set-php-56.bat; DestDir: {app}; Flags: ignoreversion
Source: {#BasePath}stubs\set-php-72.bat; DestDir: {app}; Flags: ignoreversion
Source: {#BasePath}stubs\set-php-73.bat; DestDir: {app}; Flags: ignoreversion
Source: {#BasePath}stubs\php.ini; DestDir: {app}\php56; Flags: ignoreversion
Source: {#BasePath}stubs\php.ini; DestDir: {app}\php72; Flags: ignoreversion
Source: {#BasePath}stubs\php.ini; DestDir: {app}\php73; Flags: ignoreversion
; Essential files and directories ---------------------------------------------------------------------
Source: {#BasePath}packages\php56\*; DestDir: {app}\php56; Flags: ignoreversion recursesubdirs
Source: {#BasePath}packages\php72\*; DestDir: {app}\php72; Flags: ignoreversion recursesubdirs
Source: {#BasePath}packages\php73\*; DestDir: {app}\php73; Flags: ignoreversion recursesubdirs
Source: {#BasePath}packages\ioncube\*; DestDir: {app}\ioncube; Flags: ignoreversion recursesubdirs
Source: {#BasePath}packages\composer\*; DestDir: {app}\composer; Flags: ignoreversion recursesubdirs
; Dependencies and libraries -------------------------------------------------------------------------
Source: {#BasePath}packages\vcredis\vcredis2012x64.exe; DestDir: {tmp}; Flags: ignoreversion deleteafterinstall
Source: {#BasePath}packages\vcredis\vcredis1519x64.exe; DestDir: {tmp}; Flags: ignoreversion deleteafterinstall

[Icons]
Name: "{group}\Uninstall {#AppName}"; Filename: "{uninstallexe}"

[UninstallDelete]
Type: filesandordirs; Name: {app}

[Run]
; Install external packages --------------------------------------------------------------------------
Filename: "msiexec.exe"; Parameters: "/i ""{tmp}\vcredis2012x64.exe"" /quiet /norestart"; Flags: waituntilterminated; Tasks: task_install_vcredis
Filename: "msiexec.exe"; Parameters: "/i ""{tmp}\vcredis1519x64.exe"" /quiet /norestart"; Flags: waituntilterminated; Tasks: task_install_vcredis

[Dirs]
Name: {app}\tmp; Flags: uninsalwaysuninstall

; ----------------------------------------------------------------------------------------------------
; Programmatic section -------------------------------------------------------------------------------
; ----------------------------------------------------------------------------------------------------
#include 'setup-helpers.iss'

[Code]
var
  BaseDir : String;
  Str : String;

procedure InitializeWizard;
begin
  CustomLicensePage;
  //CreateFooterText(#169 + ' 2019 - {#AppPublisher}');
  CreateFooterText('{#AppGithubUrl}');
end;

procedure ConfigureApplication;
begin
  BaseDir := ExpandConstant('{app}');

  // PHP 7.3
  FileReplaceString(BaseDir + '\php73\php.ini', '<<INSTALL_DIR>>', PathWithSlashes(ExpandConstant('{app}')));
  FileReplaceString(BaseDir + '\php73\php.ini', '<<PHP_DIR>>', PathWithSlashes(ExpandConstant('{app}\php73')));
  FileReplaceString(BaseDir + '\php73\php.ini', '<<IONCUBE_FILE>>', 'ioncube_loader_win_7.3.dll');

  // PHP 7.2
  FileReplaceString(BaseDir + '\php72\php.ini', '<<INSTALL_DIR>>', PathWithSlashes(ExpandConstant('{app}')));
  FileReplaceString(BaseDir + '\php72\php.ini', '<<PHP_DIR>>', PathWithSlashes(ExpandConstant('{app}\php72')));
  FileReplaceString(BaseDir + '\php72\php.ini', '<<IONCUBE_FILE>>', 'ioncube_loader_win_7.2.dll');

  // PHP 5.6
  FileReplaceString(BaseDir + '\php56\php.ini', '<<INSTALL_DIR>>', PathWithSlashes(ExpandConstant('{app}')));
  FileReplaceString(BaseDir + '\php56\php.ini', '<<PHP_DIR>>', PathWithSlashes(ExpandConstant('{app}\php56')));
  FileReplaceString(BaseDir + '\php56\php.ini', '<<IONCUBE_FILE>>', 'ioncube_loader_win_5.6.dll');

  // Create composer.bat
  Str := '@echo off' + #13#10#13#10 + '"'+BaseDir+'\php73\php.exe" "'+ExpandConstant('{app}\composer\composer.phar')+'" %*';
  SaveStringToFile(BaseDir + '\composer\composer.bat', Str, False);
end;

procedure CreatePathEnvironment();
begin
  EnvAddPath(ExpandConstant('{app}\php73'));
  EnvAddPath(ExpandConstant('{app}\composer'));
  EnvAddPath(ExpandConstant('{userappdata}\Composer\vendor\bin'));
end;

procedure RemovePathEnvironment;
begin
  EnvRemovePath(ExpandConstant('{app}\php73'));
  EnvRemovePath(ExpandConstant('{app}\composer'));
  EnvRemovePath(ExpandConstant('{userappdata}\Composer\vendor\bin'));
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  BaseDir := ExpandConstant('{app}');
  if CurStep = ssPostInstall then begin
    WizardForm.StatusLabel.Caption := 'Setting up application configuration ...';
    ConfigureApplication;
    if WizardIsTaskSelected('task_add_path_envars') then begin
      WizardForm.StatusLabel.Caption := 'Adding PATH environment variables ...';
      CreatePathEnvironment;
    end;
  end;
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  case CurUninstallStep of
    usUninstall:
      begin
        RemovePathEnvironment;
      end;
    usPostUninstall:
      begin
        // MsgBox(ExpandConstant('{#AppName}') + ' uninstalled, but some files are not removed!', mbInformation, MB_OK);
      end;
  end;
end;