:: �������ַ���
echo %1
:: ��Ŀ������ַ
echo %2

:: ɾ����ʷ��
del %1 /f /q /a 

:: ������
set nupkg=""

:: ���
dotnet pack %2 -c Release --output ../../tools/pack/nupkgs

:: ���°�����
for %%a in (dir /s /a /b "./nupkgs/%1") do (set nupkg=%%a)

:: ���Ͱ�
nuget push nupkgs/%nupkg% oy2eozchsd7tst7ykoaayq7oqwwehi5unkm2lwg2o7ysyu -Source https://www.nuget.org/api/v2/package