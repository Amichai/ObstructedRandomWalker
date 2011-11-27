using System;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

public class Test : Form {
	delegate void StringParameterDelegate(string value);
	Label statusIndicator;
	Label counter;
	Button button;

	/// <summary>
	/// Lock around target and currentCount
	/// </summary>
	readonly object stateLock = new object();
	int target;
	int currentCount;

	Random rng = new Random();

	Test() {
		Size = new Size(180, 120);
		Text = "Test";

		Label lbl = new Label();
		lbl.Text = "Status:";
		lbl.Size = new Size(50, 20);
		lbl.Location = new Point(10, 10);
		Controls.Add(lbl);

		lbl = new Label();
		lbl.Text = "Count:";
		lbl.Size = new Size(50, 20);
		lbl.Location = new Point(10, 34);
		Controls.Add(lbl);

		statusIndicator = new Label();
		statusIndicator.Size = new Size(100, 20);
		statusIndicator.Location = new Point(70, 10);
		Controls.Add(statusIndicator);

		counter = new Label();
		counter.Size = new Size(100, 20);
		counter.Location = new Point(70, 34);
		Controls.Add(counter);

		button = new Button();
		button.Text = "Go";
		button.Size = new Size(50, 20);
		button.Location = new Point(10, 58);
		Controls.Add(button);
		button.Click += new EventHandler(StartThread);
	}

	void StartThread(object sender, EventArgs e) {
		button.Enabled = false;
		lock (stateLock) {
			target = rng.Next(100);
		}
		Thread t = new Thread(new ThreadStart(ThreadJob));
		t.IsBackground = true;
		t.Start();
	}

	void ThreadJob() {
		MethodInvoker updateCounterDelegate = new MethodInvoker(UpdateCount);
		int localTarget;
		lock (stateLock) {
			localTarget = target;
		}
		UpdateStatus("Starting");

		lock (stateLock) {
			currentCount = 0;
		}
		Invoke(updateCounterDelegate);
		// Pause before starting
		Thread.Sleep(500);
		UpdateStatus("Counting");
		for (int i = 0; i < localTarget; i++) {
			lock (stateLock) {
				currentCount = i;
			}
			// Synchronously show the counter
			Invoke(updateCounterDelegate);
			Thread.Sleep(100);
		}
		UpdateStatus("Finished");
		Invoke(new MethodInvoker(EnableButton));
	}

	void UpdateStatus(string value) {
		if (InvokeRequired) {
			// We're not in the UI thread, so we need to call BeginInvoke
			BeginInvoke(new StringParameterDelegate(UpdateStatus), new object[] { value });
			return;
		}
		// Must be on the UI thread if we've got this far
		statusIndicator.Text = value;
	}

	void UpdateCount() {
		int tmpCount;
		lock (stateLock) {
			tmpCount = currentCount;
		}
		counter.Text = tmpCount.ToString();
	}

	void EnableButton() {
		button.Enabled = true;
	}

	static void Main() {
		Application.Run(new Test());
	}
}