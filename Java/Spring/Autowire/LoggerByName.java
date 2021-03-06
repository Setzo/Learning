package spring.testing;

public class LoggerByName {

    private LogWriter consoleWriter;
    private LogWriter fileWriter;

    public void setConsoleWriter(LogWriter writer) {
        this.consoleWriter = writer;
    }

    public void setFileWriter(LogWriter fileWriter) {
        this.fileWriter = fileWriter;
    }

    public void writeFile(String text) {
        fileWriter.write(text);
    }

    public void writeConsole(String text) {
        consoleWriter.write(text);
    }

}
